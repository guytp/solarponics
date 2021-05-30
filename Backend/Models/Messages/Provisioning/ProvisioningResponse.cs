namespace Solarponics.Models.Messages.Provisioning
{
    public class ProvisioningResponse : MessageBase
    {
        public SensorModuleConfig Config { get; set; }
        public override byte OpCode => 0x21;
    }
}