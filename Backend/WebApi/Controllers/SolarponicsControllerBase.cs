using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Solarponics.WebApi.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class SolarponicsControllerBase : ControllerBase
    {
        public int? UserId
        {
            get
            {
                var claimsPrincipal = this.User as ClaimsPrincipal;

                var claim = claimsPrincipal?.Claims?.FirstOrDefault(c => c.Type == "oid")?.Value;
                if (string.IsNullOrEmpty(claim))
                {
                    claim = claimsPrincipal?.Claims?.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
                }

                if (string.IsNullOrEmpty(claim))
                {
                    return null;
                }

                if (!int.TryParse(claim, out var userId))
                {
                    return null;
                }

                return userId;
            }
        }
    }
}