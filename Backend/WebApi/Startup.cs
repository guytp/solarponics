using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;
using Solarponics.WebApi.Data;
using Solarponics.WebApi.Swagger;
#pragma warning disable 1591

namespace Solarponics.WebApi
{
    public class Startup
    {
        private const string CorsAllowAllConfigKey = "WebApi:CorsAllowAll";
        private const string SwaggerPath = "_md";
        private const string SwaggerDocumentName = "swagger.json";
        private readonly IConfiguration _config;
        private readonly SwaggerConfig _swaggerConfig;

        public Startup(IConfiguration config)
        {
            _config = config;
            _swaggerConfig = _config.GetSection("Swagger").Get<SwaggerConfig>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseSwagger(
                c => { c.RouteTemplate = SwaggerPath + "/{documentName}/" + SwaggerDocumentName; });

            app.UseSwaggerUI(
                c =>
                {
                    c.RoutePrefix = SwaggerPath;
                    var title = _swaggerConfig.Title + " v" + _swaggerConfig.Version;

                    var rootUrl = string.Empty;
                    if (!string.IsNullOrEmpty(_swaggerConfig.OverrideRootUrl)) rootUrl = _swaggerConfig.OverrideRootUrl;

                    rootUrl += "/" + SwaggerPath + "/v" + _swaggerConfig.Version + "/" + SwaggerDocumentName;

                    c.SwaggerEndpoint(rootUrl, title);
                });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(
                endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var corsAllowAll = _config.GetValue<bool>(CorsAllowAllConfigKey);
            if (corsAllowAll)
                services.AddCors(
                    options =>
                    {
                        options.AddDefaultPolicy(
                            builder =>
                            {
                                builder.AllowAnyOrigin();
                                builder.AllowAnyMethod();
                                builder.AllowAnyHeader();
                            });
                    });

            // Disable automatic 400 responses so we can send custom ones
            services.Configure<ApiBehaviorOptions>(
                opts => { opts.SuppressModelStateInvalidFilter = true; });

            services.AddRouting();
            
            if (_config["jwtAuthentication:enabled"]?.ToLower() == "true")
            {
                var authScheme = JwtBearerDefaults.AuthenticationScheme;
                var authenticationBuilder = services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = authScheme;
                    options.DefaultChallengeScheme = authScheme;
                    options.DefaultSignInScheme = authScheme;
                });

                authenticationBuilder.AddJwtBearer(
                    options =>
                    {
                        options.Audience = _config["jwtAuthentication:audience"];
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        var key = new X509SecurityKey(
                            new X509Certificate2(Convert.FromBase64String(_config["jwtAuthentication:certificate"])));
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = _config["jwtAuthentication:issuer"],
                            RequireSignedTokens = true,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.FromMinutes(2),
                            ValidAudience = _config["jwtAuthentication:audience"],
                            IssuerSigningKey = key
                        };
                    });
            }

            services.AddMvc()
                .AddApplicationPart(Assembly.GetEntryAssembly())
                .AddControllersAsServices()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(
                c =>
                {
                    c.DocumentFilter<SwaggerBasePathFilter>();
                    c.OperationFilter<AuthResponsesOperationFilter>();
                    c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();

                    c.EnableAnnotations();
                    var info = new OpenApiInfo
                    {
                        Title = _swaggerConfig.Title, Version = "v" + _swaggerConfig.Version
                    };
                    c.SwaggerDoc("v" + _swaggerConfig.Version, info);
                    var filePath = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!,
                        Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location)!) + ".xml";
                    c.IncludeXmlComments(filePath);
                    //filePath = Path.Combine(
                    //    Path.GetDirectoryName(typeof(SensorModule).Assembly.Location)!,
                    //    Path.GetFileNameWithoutExtension(typeof(SensorModule).Assembly.Location)!) + ".xml";
                    //c.IncludeXmlComments(filePath);
                });
            services.AddSingleton(_config);
            services.AddTransient<IDatabaseConnection, DatabaseConnection>();
            services.AddSingleton<IStoredProcedureFactory, DapperStoredProcedureFactory>();
            services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
            services.AddSingleton<IJwtIssuer, JwtIssuer>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IGrainSpawnRepository, GrainSpawnRepository>();
            services.AddTransient<IRecipeRepository, RecipeRepository>();
            services.AddTransient<IHardwareRepository, HardwareRepository>();
            services.AddTransient<ISensorModuleRepository, SensorModuleRepository>();
            services.AddTransient<ISensorReadingRepository, SensorReadingRepository>();
            services.AddTransient<ICultureRepository, CultureRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IAutoclaveRepository, AutoclaveRepository>();
            services.AddTransient<IShelfRepository, ShelfRepository>();
            services.AddTransient<IWasteReasonRepository, WasteReasonRepository>();
        }
    }
}