using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using RateMyAir.API.Extensions;
using RateMyAir.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Provide a path to the Log's configuration file.
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            //services.ConfigureJwtAuthentication(Configuration);
            services.ConfigureSqlContext(Configuration);
            services.ConfigureModelValidation();
            services.ConfigureSwagger();
            services.ConfigureRepositoryManager();
            services.AddAutoMapper(typeof(Startup));

            //Add support for response formatted in Xml
            services.AddControllers(config => {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();
            //services.AddApiVersioningExtension();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //CAPIRE SE USARE QUESTO O app.UseErrorHandlingMiddleware();
            //app.ConfigureExceptionHandler(logger);
            app.UseErrorHandlingMiddleware();

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //Enable the JWT-based authentication service. 
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseAppHeaders();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwaggerExtension();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
