using System.Net;
using System.Threading.Tasks;
using Refit;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class AuthenticationApiClient : IAuthenticationApiClient
    {
        private readonly IAuthenticationApiClient refitClient;

        public AuthenticationApiClient(ProductionManagerSettings settings)
        {
            refitClient = RestService.For<IAuthenticationApiClient>(settings.ApiBaseUrl, new RefitSettings());
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            try
            {
                return await refitClient.Authenticate(request);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                    return null;
                throw;
            }
        }
    }
}