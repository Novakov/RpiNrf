using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    enum Role
    {
        RX,
        TX
    }

    class Program
    {
        static readonly byte[] Address1 = { 0xC3, 0xC3, 0xC3 };
        static readonly byte[] Address2 = { 0xC4, 0xC3, 0xC3 };
        static readonly byte[] PoisonAddress = { 0xC5, 0xC3, 0xC3 };

        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
                return;
            }

            int channel;
            if (!int.TryParse(args[0], out channel))
            {
                Usage();
                return;
            }

            if (channel != 0 && channel != 1)
            {
                Usage();
                return;
            }            

            Role role;
            if(Enum.TryParse(typeof(Role), args[1], true, out var roleObj))
            {
               role = (Role)roleObj; 
            }
            else
            {
                Usage();
                return;
            }

            Console.WriteLine("Hello World!2");

            var channels = new Dictionary<int, ChannelDef>
            {
                [0] = new ChannelDef
                {
                    Spi = Pi.Spi.Channel0,
                    ChipEnable = Pi.Gpio[WiringPiPin.Pin06],
                    Irq = Pi.Gpio[WiringPiPin.Pin05]
                },
                [1] = new ChannelDef
                {
                    Spi = Pi.Spi.Channel1,
                    ChipEnable = Pi.Gpio[WiringPiPin.Pin26],
                    Irq = Pi.Gpio[WiringPiPin.Pin27]
                }
            };

            Pi.Spi.Channel0Frequency = 2 * 1000 * 1000;
            Pi.Spi.Channel1Frequency = 2 * 1000 * 1000;

            var nrf1 = new NRFDriver(channels[0].Spi, channels[0].ChipEnable, channels[0].Irq);
            var nrf2 = new NRFDriver(channels[1].Spi, channels[1].ChipEnable, channels[1].Irq);

            nrf1.PowerDown();
            nrf2.PowerDown();

            Thread.Sleep(100);

            nrf1.PowerUp();
            nrf2.PowerUp();

            Thread.Sleep(100);
            
            nrf1.Setup();
            nrf2.Setup();

            nrf2.EnableRX(1, Address2);
            nrf2.EnableRX(3, PoisonAddress);

            var txTask = Task.Run(async () => await RunTransmitter(nrf1));

            var rxTask = Task.Run(() => RunReceiver(nrf2));

            await Task.WhenAll(new[] { txTask, rxTask });
        }

        private static void Usage()
        {
            Console.WriteLine("RpiNrf <channel> <role>");
            Console.WriteLine();
            Console.WriteLine("\tchannel\tSPI channel to use (0 or 1)");
            Console.WriteLine("\rrole\tRole to execute (tx or rx)");
        }

        private static string AsHex(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(x => x.ToString("X2")));
        }

        private static void Inc(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i]++;
            }
        }

        private static void RunReceiver(NRFDriver nrf2)
        {
            nrf2.EnableReceiving();

            while (true)
            {
                Console.WriteLine($"Status2 = {nrf2.ReadStatus():X}");
                var (pipe, frame) = nrf2.ReceiveFrame().Value;

                Console.WriteLine($"Received {AsHex(frame)} bytes on pipe {pipe}");

                if (pipe == 3)
                {
                    Console.WriteLine("Poison detected");
                    break;
                }
            }
        }

        private static async Task RunTransmitter(NRFDriver nrf)
        {
            var data = Enumerable.Range(100, 32).Select(x => (byte)x).ToArray();
            Console.WriteLine("Waiting before send");
            await Task.Delay(2000);
            Console.WriteLine("Sending");
            for (int i = 0; i < 20; i++)
            {
                Inc(data);
                nrf.Transmit(Address2, data);
                Console.WriteLine($"Status1 = {nrf.ReadStatus():X}");
                await Task.Delay(250);
            }

            data[0] = 0xDE;
            data[1] = 0xAD;
            data[2] = 0xBE;
            data[3] = 0xEF;
            var st = nrf.Transmit(PoisonAddress, data);
            Console.WriteLine($"Sending done 0x{st:X2}");
        }
    }
}
