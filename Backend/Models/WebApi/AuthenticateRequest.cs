namespace Solarponics.Models.WebApi
{
    public class AuthenticateRequest
    {
        public short UserId { get; set; }
        public short Pin { get; set; }
    }
}