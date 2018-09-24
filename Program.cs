using System;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!2");

            var pin = Pi.Gpio[WiringPiPin.Pin01];

            pin.PinMode = GpioPinDriveMode.Output;

            while(true)
            {
                Console.WriteLine("loop");
                pin.Write(true);
                Thread.Sleep(500);
                pin.Write(false);
                Thread.Sleep(500);
            }
        }
    }
}
