using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Data;
using System.Net;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.ApiClient
{
    public class SensorReadingApiClient : ISensorReadingApiClient
    {
        private readonly ISensorReadingApiClient refitClient;

        public SensorReadingApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<ISensorReadingApiClient>(settings.ApiBaseUrl, refitSettings);
        }

        public async Task<SensorReading> GetCurrent(int id)
        {
            try
            {
                return await this.refitClient.GetCurrent(id);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return default;
                }

                throw;
            }
        }
        public async Task<AggregateSensorReading[]> GetAggregate(int id, AggregateTimeframe timeframe)
        {
            return await this.refitClient.GetAggregate(id, timeframe);
        }
    }
}