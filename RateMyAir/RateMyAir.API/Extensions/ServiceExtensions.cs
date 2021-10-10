using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces;
using RateMyAir.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RateMyAir.API.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// IIS integration for deploying on IIS
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        /// <summary>
        /// Injects the Logger service as a singleton service
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// Enable CORS (Cross Origin Resource Sharing)
        /// Mechanism to give or restrict access rights to applications from different domains.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",

                    //Allows requests from any source
                    builder => builder.AllowAnyOrigin()
                    //Allows requests only from mydomain
                    //builder.WithOrigins("https://mydomain.com")

                    //Allows all HTTP methods
                    .AllowAnyMethod()
                    //Allows only specified HTTP methods
                    //.WithMethods("GET", "POST")

                    //Allows all headers
                    .AllowAnyHeader()
                    //Allows only specific headers
                    //.WithHeaders("accept", "content- type")
                );
            });
        }

        /// <summary>
        /// Register swagger in order to generate API documentation
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "RateMyAir API",
                    Version = "v1",
                    Description = "",
                    TermsOfService = new Uri("https://github.com/Gbertaz"),
                    Contact = new OpenApiContact { Name = "RateMyAir", Email = "nottheworstdev@gmail.com", Url = new Uri("https://github.com/Gbertaz") },
                    License = new OpenApiLicense { Name = "Use under LICX", Url = new Uri("https://github.com/Gbertaz") }
                });
                c.CustomSchemaIds(i => i.FullName);

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization by x-api-key inside request's header",
                    Scheme = "ApiKeyScheme"
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    Name = "ApiKey",
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                   { key, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);

                // Set the documentation file path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }


        /// <summary>
        /// Automatic model validation without the need to add the validation code in every API action
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureModelValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //Disable the internal model state filter
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(Filters.ValidateModelAttribute));
            });
        }

        /// <summary>
        /// Register the database context with dependency injection
        /// Automatically reads the connection string from the appsettings.json
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(config.GetConnectionString("DefaultConnection")));
        }

        /// <summary>
        /// Repositories configuration
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            //services.AddVersionedApiExplorer(config =>
            //{
            //    //The format of the version added to the route URL
            //    config.GroupNameFormat = "'v'VVV";

            //    config.AddApiVersionParametersWhenVersionNeutral = true;

            //    config.AssumeDefaultVersionWhenUnspecified = true;
            //    config.DefaultApiVersion = new ApiVersion(1, 0);

            //    //Tells swagger to replace the version in the controller route
            //    config.SubstituteApiVersionInUrl = true;
            //});

            services.AddApiVersioning(config =>
            {
                // Advertise the API versions supported for the particular endpoint adding the headers in the API response
                config.ReportApiVersions = true;

                // If the client hasn't specified the API version in the request, use the default API version number
                config.AssumeDefaultVersionWhenUnspecified = true;

                // Specify the default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0);

                // Header versioning
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");

                // Supporting multiple versioning scheme
                //config.ApiVersionReader = ApiVersionReader.Combine(
                //    new QueryStringApiVersionReader("v"),
                //    new HeaderApiVersionReader("api-version"));
            });
        }

    }
}
