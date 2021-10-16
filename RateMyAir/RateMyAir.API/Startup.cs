using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using RateMyAir.API.Extensions;
using System.IO;

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
            services.AddMemoryCache();
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureServices();
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
            services.AddApiVersioningExtension();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseErrorHandlingMiddleware();
            //app.UseApiKeyMiddleware();
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
