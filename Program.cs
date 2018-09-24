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

            Pi.Spi.Channel0Frequency = 2 * 1000 * 1000;

            var spi = Pi.Spi.Channel0;

            spi.Write(new byte[] {1,2,3});
        }
    }
}
