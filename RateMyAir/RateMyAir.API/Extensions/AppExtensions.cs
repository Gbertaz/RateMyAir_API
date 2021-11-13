using Microsoft.AspNetCore.Builder;
using RateMyAir.API.Middlewares;
using System.Linq;

namespace RateMyAir.API.Extensions
{
    public static class AppExtensions
    {
        /// <summary>
        /// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "RateMyAir API");
            });
        }

        /// <summary>
        /// This will forward proxy headers to the current request
        /// This is necessary for example to get the client IP Address from the Headers of the call
        /// </summary>
        /// <param name="app"></param>
        public static void UseAppHeaders(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });

            app.Use(async (context, next) =>
            {
                var forwardedPath = context.Request.Headers["X-Forwarded-Path"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedPath))
                {
                    context.Request.PathBase = forwardedPath;
                }

                await next();
            });
        }

        /// <summary>
        /// Global error handler
        /// </summary>
        /// <param name="app"></param>
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }

        /// <summary>
        /// ApiKey Authentication 
        /// </summary>
        /// <param name="app"></param>
        public static void UseApiKeyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
