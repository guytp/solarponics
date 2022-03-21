using System.Threading.Tasks;
using Refit;
using Solarponics.Models.WebApi;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface IAuthenticationApiClient
    {
        [Put("/auth")]
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }
}