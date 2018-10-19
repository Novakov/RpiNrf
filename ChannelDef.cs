using Unosquare.RaspberryIO.Gpio;

namespace RpiNrf
{
    public class ChannelDef
    {
        public SpiChannel Spi { get; set; }
        public GpioPin ChipEnable { get; set; }
        public GpioPin Irq { get; set; }
    }
}