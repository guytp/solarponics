using System;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Domain
{
    public class AuthenticationSession : IAuthenticationSession
    {
        public User User { get; private set; }
        public AuthenticationToken Token { get; private set; }

        public event EventHandler Login;
        public event EventHandler Logout;

        public void SetUser(User user, AuthenticationToken token)
        {
            User = user;
            Token = token;
            if (user == null)
                Logout?.Invoke(this, new System.EventArgs());
            else
                Login?.Invoke(this, new System.EventArgs());
        }
    }
}