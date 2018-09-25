using System;
using System.Linq;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    class Program
    {
        static readonly byte[] Address1 = { 0xC3, 0xC3, 0xC3 };
        static readonly byte[] Address2 = { 0xC3, 0xC3, 0xC4 };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!2");

            Pi.Spi.Channel0Frequency = 2 * 1000 * 1000;
            Pi.Spi.Channel1Frequency = 2 * 1000 * 1000;

            var nrf1 = new NRFDriver(Pi.Spi.Channel0, Pi.Gpio[WiringPiPin.Pin06]);
            var nrf2 = new NRFDriver(Pi.Spi.Channel1, Pi.Gpio[WiringPiPin.Pin26]);

            Console.WriteLine($"Status = {nrf1.ReadStatus():X} Status = {nrf2.ReadStatus():X}");
            nrf1.FlushTX();
            Console.WriteLine($"Status = {nrf1.ReadStatus():X} Status = {nrf2.ReadStatus():X}");

            nrf1.Setup();
            nrf2.Setup();

            nrf1.EnableRX(1, Address1);
            nrf2.EnableRX(1, Address2);

            nrf2.EnableRXMode();

            Thread.Sleep(1000);

            var data = Enumerable.Range(100, 32).Select(x => (byte)x).ToArray();

            
            nrf1.Transmit(Address2, data);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Status = {nrf1.ReadStatus():X}\t{nrf2.ReadStatus():X}\t{nrf1.ObserveTX()}");
                Thread.Sleep(1000);
            }
        }
    }
}
