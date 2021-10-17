using System;
using Solarponics.Models;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IAuthenticationSession
    {
        User User { get; }
        AuthenticationToken Token { get; }
        event EventHandler LoggedIn;
        event EventHandler LoggedOut;

        void Login(User user, AuthenticationToken token, short userId, short pin);
        void Logout();
    }
}