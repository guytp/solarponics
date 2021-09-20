using System;
using Solarponics.Models;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IAuthenticationSession
    {
        User User { get; }
        AuthenticationToken Token { get; }
        event EventHandler Login;
        event EventHandler Logout;

        void SetUser(User user, AuthenticationToken token);
    }
}