using System;
using System.Net.Http;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Marvin.Cache.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OrderApplication.ActionFilters;
using OrderApplication.Exceptions;
using OrderApplication.Models.DTO;
using OrderApplication.Validation;
using OrderDomain.ValueObjects;
using Polly;
using Serilog;

namespace OrderApplication.Services
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
            services.AddTransient<IValidator<OrderForCreation>, OrderForCreateValidation>();
            services.AddTransient<IValidator<OrderForUpdate>, OrderForUpdateValidation>();
            services.AddTransient<IValidator<ProductForUpdate>, ProductForUpdateValidation>();
            services.AddTransient<IValidator<ProductForCreation>, ProductForCreateValidation>();
            services.AddTransient<IValidator<Address>, AddressValidation>();
            
            //ActionFilters
            services.AddScoped<ProductExistAttribute>();
            services.AddScoped<CustomerExistAttribute>();
            
            //  Http Client Service
            services.AddHttpClient("httpClient")
                 .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
                 {
                     logger.Error("CUSTOMER SERVICE IS NOT RESPONDING");
                     throw new ServerNotRespondingException(
                         "External server took so much time to respond");

                 }).AsAsyncPolicy<HttpResponseMessage>())
                 .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
                 {
                     logger.Warning($"CUSTOMER SERVICE IS NOT RESPONDING. TRYING AGAIN: {r}");
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
                    HealthStatus.Unhealthy | HealthStatus.Degraded,
                    new string[]{"mongo","db"},
                    timeout:TimeSpan.FromSeconds(5));
            return services;
        }
    }
}