using System;
using System.Threading.Tasks;
using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IAuthenticationApiClient
    {
        [Put("/auth")]
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }
}