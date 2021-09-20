using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models.WebApi;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtIssuer jwtIssuer;

        public AuthenticationController(IUserRepository userRepository, IJwtIssuer jwtIssuer)
        {
            this.userRepository = userRepository;
            this.jwtIssuer = jwtIssuer;
        }

        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(AuthenticateResponse))]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateRequest request)
        {
            var user = await userRepository.Authenticate(request.UserId, request.Pin);
            if (user == null)
            {
                return Unauthorized();
            }
            
            var token = this.jwtIssuer.CreateToken(user);

            var response = new AuthenticateResponse
            {
                User = user, AuthenticationToken = token
            };

            return Ok(response);
        }
    }
}