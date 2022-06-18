using System.IO;
using System.Linq;
using System.Reflection;
using CustomerApplication.Exceptions;
using CustomerApplication.Services;
using CustomerInfrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;


namespace CustomerPresentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adding Controller
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => 
                {
                    //Set the action of API if model is invalid.
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var messages = context.ModelState.Values
                            .Where(x => x.ValidationState == ModelValidationState.Invalid)
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage)
                            .ToList();
                        throw new InvalidModelException(string.Join($" , ", messages));
                        
                    };
                });
            //Adding services
            services.AddApplicationServices(Configuration);
            services.AddInfrastructureServices(Configuration);
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CustomerPresentation", Version = "v1"});
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                var path =  Path.Combine(basePath, fileName);
                c.IncludeXmlComments(path);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerPresentation v1"));
            }

            app.UseCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.RunCachingMiddleware();
            app.RunHealthChecks();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}