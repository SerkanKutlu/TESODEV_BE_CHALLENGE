using System;
using System.Net.Http;
using System.Reflection;
using CustomerApplication.Exceptions;
using CustomerApplication.Models.DTO;
using CustomerApplication.Validation;
using Marvin.Cache.Headers;
using CustomerDomain.ValueObjects;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Serilog;

namespace CustomerApplication.Services
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Serilog
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            Log.Logger = logger;
            //Adding Validations
            services.AddFluentValidation();
            services.AddTransient<IValidator<CustomerForCreation>, CustomerForCreationValidation>();
            services.AddTransient<IValidator<CustomerForUpdate>, CustomerForUpdateValidation>();
            services.AddTransient<IValidator<Address>, AddressValidation>();
            //Http Client Service (Using "Polly" to add policy)
            services.AddHttpClient("httpClient")
                .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
                {
                    logger.Error("ORDER SERVICE IS NOT RESPONDING");
                    throw new ServerNotRespondingException(
                        "External server took so much time to respond");

                }).AsAsyncPolicy<HttpResponseMessage>())
                .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
                {
                    logger.Warning($"ORDER SERVICE IS NOT RESPONDING. TRYING AGAIN: {r}");
                }));
            
            //Adding caching service
            services.AddResponseCaching();
            //Adding caching headers
            services.AddHttpCacheHeaders((expirationOpt) =>
                {
                    expirationOpt.MaxAge = 120;
                    expirationOpt.CacheLocation = CacheLocation.Private;
                },
                (validationOpt) =>
                {
                    validationOpt.MustRevalidate = true;
                });
            services.AddHttpContextAccessor();
            
            //HealthCheck Configurations
            var value = configuration.GetSection("MongoSettings").GetSection("ConnectionString").Value;
            services.AddHealthChecks()
                .AddMongoDb(configuration.GetSection("MongoSettings").GetSection("ConnectionString").Value,
                    "MONGO DB CHECK",
                    HealthStatus.Unhealthy | HealthStatus.Degraded, //Not working or not working smoothly 
                    new string[]{"mongo","db"},
                    timeout:TimeSpan.FromSeconds(5));
            return services;
        }
        
    }
}