namespace Solarponics.Models.Messages.Provisioning
{
    public class ProvisioningRequest : MessageBase
    {
        public string SerialNumber { get; set; }
        public override byte OpCode => 0x20;
    }
}