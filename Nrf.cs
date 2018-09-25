using System;
using System.Linq;
using System.Threading;
using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    class NRFDriver
    {
        private readonly SpiChannel spi;
        private readonly GpioPin cePin;

        public NRFDriver(SpiChannel spi, GpioPin cePin)
        {
            this.spi = spi;
            this.cePin = cePin;
            this.cePin.PinMode = GpioPinDriveMode.Output;
            this.cePin.Write(false);
        }

        public byte ReadStatus()
        {
            return ReadRegister(Register.STATUS, 1)[0];
        }

        public byte ObserveTX()
        {
            return ReadRegister(Register.OBSERVE_TX, 1)[0];
        }

        public void Transmit(byte[] address, byte[] data)
        {
            {
                var config = (ConfigRegister)ReadRegister(Register.CONFIG, 1)[0];
                config = config & ~ConfigRegister.PRIM_RX;
                WriteRegister(Register.CONFIG, (byte)config);
            }

            // write TX addr
            WriteRegister(Register.TX_ADDR, address);
            WriteRegister(Register.RX_ADDR_P0, address);
            
            // write TX pld
            SendCommand(Command.WriteTXPayload, data);

            // CE high
            this.cePin.Write(true);
            // sleep
            Thread.Sleep(1000);
            // CE low
            this.cePin.Write(false);

            {
                var config = (ConfigRegister)ReadRegister(Register.CONFIG, 1)[0];
                config = config | ConfigRegister.PRIM_RX;
                WriteRegister(Register.CONFIG, (byte)config);
            }
        }

        internal void EnableRXMode()
        {
            this.cePin.Write(true);
        }

        internal void EnableCarrier()
        {
            WriteRegister(Register.RF_SETUP, (1 << 7) |(1 << 4));

            // CE high
            this.cePin.Write(true);
            // sleep
            Thread.Sleep(10000);
            // CE low
            this.cePin.Write(false);
        }

        public void FlushTX()
        {
            SendCommand(Command.FlushTX);
            var s = ReadStatus();
            WriteRegister(Register.STATUS, s);
        }

        public void Setup()
        {
            WriteRegister(Register.CONFIG, (byte)(ConfigRegister.PWR_UP | ConfigRegister.EN_CRC | ConfigRegister.CRC0 | ConfigRegister.PRIM_RX));
            WriteRegister(Register.EN_AA, 1 << 0);
            WriteRegister(Register.EN_RXADDR, 1 << 0);
            WriteRegister(Register.SETUP_AW, 0b01); // 3 byte address
            WriteRegister(Register.SETUP_RETR, (0b1111 << 4) | (0b1111 << 4));
            WriteRegister(Register.RF_CH, 1);
            WriteRegister(Register.RF_SETUP, (byte) (RFSetupRegister.RF_DR_LOW | RFSetupRegister.RF_PWR0));
        }

        public void EnableRX(int pipe, byte[] address)
        {
            // EN_AA
            var aa = ReadRegister(Register.EN_AA, 1)[0];
            aa |= (byte) (1 << pipe);
            WriteRegister(Register.EN_AA, aa);

            // EN_RXADDR
            var rxaddr = ReadRegister(Register.EN_RXADDR, 1)[0];
            rxaddr |= (byte) (1 << pipe);
            WriteRegister(Register.EN_RXADDR, rxaddr);

            // RX_ADDR_Px
            if (pipe == 1)
            {
                // full address for pipes 0 and 1
                WriteRegister((Register)((byte)Register.RX_ADDR_P0 + pipe), address);
            }
            else
            {
                WriteRegister((Register)((byte)Register.RX_ADDR_P0 + pipe), address.Last());
            }

            WriteRegister((Register)((byte)Register.RX_PW_P0 + pipe), 32);
        }

        private byte[] ReadRegister(Register register, int size)
        {
            var command = new byte[1 + size];
            command[0] = (byte)register;

            var result = spi.SendReceive(command);

            return result.Skip(1).ToArray();
        }

        private void WriteRegister(Register register, params byte[] data)
        {
            var command = new byte[1 + data.Length];
            command[0] = (byte) (0b00100000 | (byte)register);
            data.CopyTo(command, 1);

            spi.Write(command);
        }

        private void SendCommand(Command cmd, params byte[] data)
        {
            var command = new byte[1 + data.Length];
            command[0] = (byte)cmd;
            data.CopyTo(command, 1);

            spi.Write(command);
        }

        private enum Register: byte
        {
            STATUS = 0x07,
            TX_ADDR = 0x10,
            CONFIG = 0x00,
            RF_CH = 0x05,
            RF_SETUP = 0x6,
            SETUP_AW = 0x3,
            EN_AA = 0x1,
            SETUP_RETR = 0x4,
            EN_RXADDR = 0x2,
            RX_ADDR_P0 = 0x0A,
            OBSERVE_TX = 0x8,
            RX_PW_P0 = 0x11
        }

        private enum Command: byte
        {
            WriteTXPayload = 0b10100000,
            FlushTX = 0b11100001
        }

        [Flags]
        private enum ConfigRegister: byte
        {
            PRIM_RX = 1 << 0,
            PWR_UP = 1 << 1,
            CRC0 = 1 << 2,
            EN_CRC = 1 << 3,
        }

        [Flags]
        private enum RFSetupRegister: byte
        {
            RF_DR_LOW = 1 << 5,
            RF_PWR0 = 1 << 1,
            RF_PWR1 = 1 << 2,
        }
    }
}