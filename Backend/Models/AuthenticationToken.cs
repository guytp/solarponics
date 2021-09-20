using System;

namespace Solarponics.Models
{
    public class AuthenticationToken
    {
        public DateTime Expires { get; set; }

        public string Token { get; set; }
    }
}