using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Solarponics.WebApi.Swagger
{
    internal sealed class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo?.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            var hasAuthorizeAttributeApplied = authAttributes != null && authAttributes.Any();
            if (!hasAuthorizeAttributeApplied)
            {
                return;
            }

            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
                    },
                    new string[] {}
                }
            });
        }
    }
}