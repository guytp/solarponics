#pragma warning disable 1591
namespace Solarponics.WebApi.Swagger
{
    public class SwaggerConfig
    {
        public string Title { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// Specify the URL prefix to use when constructing Swagger documents.  If this is null or empty the current server and the root of it will be used.  Set this to a value when the API
        /// is sitting behind an API gateway, reverse proxy, etc. and you want the public facing address returned.  This impacts the Swagger UI and not the contents of the JSON.
        /// </summary>
        public string OverrideRootUrl { get; set; }

        /// <summary>
        /// Specify optional settings to change how the servers are added to Swagger.json.
        /// </summary>
        public SwaggerBasePathOverrides BasePathOverrides { get; set; }
    }
}