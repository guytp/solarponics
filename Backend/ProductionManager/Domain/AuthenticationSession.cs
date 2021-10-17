using System;
using System.Timers;
using Serilog;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;

namespace Solarponics.ProductionManager.Domain
{
    public class AuthenticationSession : IAuthenticationSession
    {
        private short userId;
        private short pin;
        private Timer timer;
        private readonly IAuthenticationApiClient apiClient;

        public User User { get; private set; }

        public AuthenticationToken Token { get; private set; }

        public event EventHandler LoggedIn;
        public event EventHandler LoggedOut;

        public AuthenticationSession(IAuthenticationApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public void Logout()
        {
            User = null;
            Token = null;
            LoggedOut?.Invoke(this, new System.EventArgs());
            this.ResetTimer();
        }

        public void Login(User user, AuthenticationToken token, short userId, short pin)
        {
            this.CoreLogin(user, token, userId, pin);
            LoggedIn?.Invoke(this, new System.EventArgs());
        }

        private void CoreLogin(User user, AuthenticationToken token, short userId, short pin)
        {
            User = user;
            Token = token;
            this.userId = userId;
            this.pin = pin;
            var expiryRetry = token.Expires.Subtract(DateTime.UtcNow).TotalMilliseconds * 0.9;
            this.ResetTimer();
            this.timer = new Timer(expiryRetry)
            {
                AutoReset = false,
                Enabled = true
            };
            this.timer.Elapsed += OnElapsed;
        }

        private async void OnElapsed(object sender, ElapsedEventArgs e)
        {
            if (User == null || Token == null)
                return;


            if (Token.Expires < DateTime.UtcNow)
            {
                Logout();
                return;
            }

            try
            {

                var request = new AuthenticateRequest
                {
                    UserId = userId,
                    Pin = pin
                };
                var authenticateResponse = await apiClient.Authenticate(request);
                if (request == null)
                    throw new Exception("Re-authentication failed with null response");

                this.CoreLogin(authenticateResponse.User, authenticateResponse.AuthenticationToken, userId, pin);
                return;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to renew JWT");
            }

            this.timer.Interval = this.Token.Expires.Subtract(DateTime.UtcNow).TotalMilliseconds * 0.9;
            if (this.timer.Interval < 30000)
            {
                this.timer.Interval = 30000;
            }
            this.timer.Enabled = true;
        }

        private void ResetTimer()
        {
            if (timer == null)
                return;
            timer.Elapsed -= OnElapsed;
            timer.Stop();
            timer.Dispose();
            timer = null;
        }
    }
}