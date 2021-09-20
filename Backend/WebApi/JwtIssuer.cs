using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Solarponics.Models;

namespace Solarponics.WebApi
{
    internal class JwtIssuer : IJwtIssuer
    {
        private readonly string audience;
        private readonly string issuer;

        private readonly X509SecurityKey key;

        private readonly int lifetimeInSeconds;

        public JwtIssuer(IConfiguration config)
        {
            this.issuer = config.GetValue<string>("authSigning:issuer");
            this.audience = config.GetValue<string>("authSigning:audience");
            this.lifetimeInSeconds = config.GetValue<int>("authSigning:lifetime");
            var certificate = config.GetValue<string>("authSigning:certificate");
            var certificatePassword = config.GetValue<string>("authSigning:certificatePassword");
            this.key = new X509SecurityKey(new X509Certificate2(Convert.FromBase64String(certificate), certificatePassword));
        }

        public AuthenticationToken CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsList = new List<Claim>();
            if (!string.IsNullOrEmpty(user.Name))
            {
                claimsList.Add(new Claim(ClaimTypes.Name, user.Name));
            }
            
            if (user.Roles != null)
            {
                claimsList.AddRange(user.Roles.Distinct().Select(role => new Claim(ClaimTypes.Role, role)));
            }

            claimsList.Add(new Claim("oid", user.Id.ToString()));

            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(this.lifetimeInSeconds);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = this.audience,
                IssuedAt = now,
                Subject = new ClaimsIdentity(claimsList.ToArray()),
                Expires = expires,
                Issuer = this.issuer,
                SigningCredentials = new SigningCredentials(this.key, SecurityAlgorithms.RsaSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new AuthenticationToken
            {
                Expires = expires, Token = jwt
            };
        }
    }
}
