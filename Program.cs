using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    class Program
    {
        static readonly byte[] Address1 = { 0xC3, 0xC3, 0xC3 };
        static readonly byte[] Address2 = { 0xC4, 0xC3, 0xC3 };
        static readonly byte[] PoisonAddress = { 0xC5, 0xC3, 0xC3 };

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!2");

            Pi.Spi.Channel0Frequency = 2 * 1000 * 1000;
            Pi.Spi.Channel1Frequency = 2 * 1000 * 1000;

            var nrf1 = new NRFDriver(Pi.Spi.Channel0, Pi.Gpio[WiringPiPin.Pin06], Pi.Gpio[WiringPiPin.Pin05]);
            var nrf2 = new NRFDriver(Pi.Spi.Channel1, Pi.Gpio[WiringPiPin.Pin26], Pi.Gpio[WiringPiPin.Pin27]);

            Console.WriteLine($"Status = {nrf1.ReadStatus():X} Status = {nrf2.ReadStatus():X}");
            nrf1.Flush();
            nrf2.Flush();
            Console.WriteLine($"Status = {nrf1.ReadStatus():X} Status = {nrf2.ReadStatus():X}");

            nrf1.Setup();
            nrf2.Setup();

            // nrf1.EnableCarrier();

            nrf1.EnableRX(1, Address1);
            nrf2.EnableRX(1, Address2);
            nrf2.EnableRX(3, PoisonAddress);

            nrf2.EnableReceiving();

            Thread.Sleep(1000);

            var data = Enumerable.Range(100, 32).Select(x => (byte)x).ToArray();

            var txTask = Task.Run(async () =>
            {
                Console.WriteLine("Waiting before send");
                await Task.Delay(2000);
                Console.WriteLine("Sending");
                for (int i = 0; i < 20; i++)
                {
                    Inc(data);

                    nrf1.Transmit(Address2, data);
                    Console.WriteLine($"Status1 = {nrf1.ReadStatus():X}");
                    await Task.Delay(250);
                }

                data[0] = 0xDE;
                data[1] = 0xAD;
                data[2] = 0xBE;
                data[3] = 0xEF;
                var st = nrf1.Transmit(PoisonAddress, data);
                Console.WriteLine($"Sending done 0x{st:X2}");
            });

            var rxTask = Task.Run(() => 
            {
                while(true)
                {
                    Console.WriteLine($"Status2 = {nrf2.ReadStatus():X}");
                    var (pipe, frame) = nrf2.ReceiveFrame().Value;

                    Console.WriteLine($"Received {AsHex(frame)} bytes on pipe {pipe}");

                    //if (frame[0] == 0xDE && frame[1] == 0xAD && frame[2] == 0xBE && frame[3] == 0xEF)
                    if (pipe == 3)
                    {
                        Console.WriteLine("Poison detected");
                        break;
                    }
                }
            });
            
            await Task.WhenAll(new[]{ txTask, rxTask });
        }

        private static string AsHex(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(x=>x.ToString("X2")));
        }

        private static void Inc(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i]++;
            }
        }
    }
}
