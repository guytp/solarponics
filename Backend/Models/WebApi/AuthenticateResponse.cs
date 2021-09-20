namespace Solarponics.Models.WebApi
{
    public class AuthenticateResponse
    {
        public AuthenticationToken AuthenticationToken { get; set; }
        public User User { get; set; }
    }
}