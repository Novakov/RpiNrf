using System.Linq;
using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    class NRFDriver
    {
        private readonly SpiChannel spi;

        public NRFDriver(SpiChannel spi)
        {
            this.spi = spi;
        }

        public int ReadStatus()
        {
            return ReadRegister(Register.STATUS, 1)[0];
        }

        private byte[] ReadRegister(Register register, int size)
        {
            var command = new byte[1 + size];
            command[0] = (byte)register;

            var result = spi.SendReceive(command);

            return result.Skip(1).ToArray();
        }

        private enum Register: byte
        {
            STATUS = 0x07
        }
    }
}