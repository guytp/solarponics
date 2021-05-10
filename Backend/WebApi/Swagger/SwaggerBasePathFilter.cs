using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
#pragma warning disable 1591

namespace Solarponics.WebApi.Swagger
{
    /// <summary>
    ///     This filter allows the root path of the URL to be rewritten to cope with being behind a load balancer - i.e. if we
    ///     are at /api-name rather than /.  It
    ///     also allows hostname to be updated.
    /// </summary>
    public class SwaggerBasePathFilter : IDocumentFilter
    {
        private readonly SwaggerConfig _config;

        public SwaggerBasePathFilter(IConfiguration config)
        {
            _config = config.GetSection("Swagger").Get<SwaggerConfig>();
        }


        public SwaggerBasePathFilter(SwaggerConfig config)
        {
            _config = config;
        }

        /// <summary>
        ///     Applies this filter to the current document.
        /// </summary>
        /// <param name="swaggerDoc">
        ///     The swagger document to apply from.
        /// </param>
        /// <param name="context">
        ///     The current document context.
        /// </param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (_config.BasePathOverrides == null || string.IsNullOrEmpty(_config.BasePathOverrides.Host)) return;

            var portComponent = string.Empty;
            if (_config.BasePathOverrides.Port.HasValue) portComponent = ":" + _config.BasePathOverrides.Port.Value;

            var scheme = "http";
            if (!string.IsNullOrEmpty(_config.BasePathOverrides.Scheme)) scheme = _config.BasePathOverrides.Scheme;

            var pathPortSeparator = string.Empty;
            if (!string.IsNullOrEmpty(_config.BasePathOverrides.Path) && !_config.BasePathOverrides.Path.StartsWith("/")
            ) pathPortSeparator = "/";

            swaggerDoc.Servers.Add(new OpenApiServer
            {
                Url = scheme + "://" + _config.BasePathOverrides.Host + portComponent + pathPortSeparator +
                      _config.BasePathOverrides.Path
            });
        }
    }
}