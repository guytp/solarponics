using System.Threading.Tasks;
using Solarponics.Models.Messages;
using Solarponics.Models.Messages.Provisioning;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Exceptions;

namespace Solarponics.ProvisioningServer.MessageHandlers
{
    public class ProvisioningRequestMessageHandler : IMessageHandler
    {
        private readonly IProvisioningRepository _repo;

        public ProvisioningRequestMessageHandler(IProvisioningRepository repo)
        {
            _repo = repo;
        }

        public async Task<IMessage> Handle(IMessage inbound, INetworkSession session)
        {
            var request = (ProvisioningRequest) inbound;

            var config = await _repo.GetConfig(request.SerialNumber);
            if (config == null)
                throw new ClientException("urn:sp:provisioning:unknown-serial-number",
                    "The supplied serial number is not available for provisioning");

            await _repo.Provision(request.SerialNumber);

            return new ProvisioningResponse
            {
                Config = config
            };
        }
    }
}