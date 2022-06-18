using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Creators;
using CustomerApplication.Interfaces.Helpers;
using CustomerApplication.Interfaces.Repository;
using CustomerApplication.Interfaces.Services;
using CustomerApplication.Interfaces.Settings;
using CustomerInfrastructure.Creators;
using CustomerInfrastructure.Database;
using CustomerInfrastructure.Helpers;
using CustomerInfrastructure.Repository;
using CustomerInfrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CustomerInfrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Adding MongoSettings object to IOC container by filling MongoSettings at "appsettings.json" file
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));
            services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
            services.AddTransient<IMongoService, MongoService>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ICustomerUpdateHelper,CustomerUpdateHelper>();
            //Adding HttpRequestSettings object to IOC container
            services.Configure<HttpRequestSettings>(configuration.GetSection(nameof(HttpRequestSettings)));
            services.AddSingleton<IHttpRequestSettings>(provider =>
                provider.GetRequiredService<IOptions<HttpRequestSettings>>().Value);
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddSingleton<ICustomerUpdateHelper,CustomerUpdateHelper>();
            services.AddSingleton<ICustomerChainCreator,CustomerChainCreator>();
            return services;
        }
        
    }
}