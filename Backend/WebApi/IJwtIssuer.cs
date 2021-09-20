using Solarponics.Models;

namespace Solarponics.WebApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IJwtIssuer
    {
        AuthenticationToken CreateToken(User user);
    }
}