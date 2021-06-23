using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(SensorModule[]))]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateRequest request)
        {
            var result = await userRepository.Authenticate(request.UserId, request.Pin);
            return result == null ? Unauthorized() : (IActionResult) Ok(result);
        }
    }
}