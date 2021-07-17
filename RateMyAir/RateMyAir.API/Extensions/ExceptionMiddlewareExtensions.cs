using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using RateMyAir.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RateMyAir.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"An error occurred: {contextFeature.Error}");

                        //ErrorDetails details = new ErrorDetails();
                        //details.StatusCode = context.Response.StatusCode;
                        //details.Message = "Internal Server Error";

                        //await context.Response.WriteAsync(details.ToString());
                        await context.Response.WriteAsync("");
                    }
                });
            });
        }
    }
}
