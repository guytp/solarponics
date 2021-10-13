using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        protected (bool, ValidationResult[]) TryGetValidationErrors(object objectToValidate)
        {
            var context = new ValidationContext(objectToValidate);
            var resultsList = new List<ValidationResult>();
            var returnResult = Validator.TryValidateObject(objectToValidate, context, resultsList, true);
            return (returnResult, resultsList.ToArray());
        }

        protected IActionResult ValidationFailure(string message, string memberName)
        {
            var validationResult = new ValidationResult(
                message,
                new[]
                {
                    memberName
                });

            return this.ValidationFailure(
                new[]
                {
                    validationResult
                });
        }

        protected IActionResult ValidationFailure(ValidationResult[] validationFailures)
        {
            return this.BadRequest(new ValidationResponse(validationFailures));
        }
    }
}