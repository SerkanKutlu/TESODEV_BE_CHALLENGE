using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderApplication.Interfaces.Creators;
using OrderApplication.Interfaces.Helper;
using OrderApplication.Interfaces.Repository;
using OrderApplication.Interfaces.Services;
using OrderApplication.Interfaces.Settings;
using OrderInfrastructure.Creators;
using OrderInfrastructure.Database;
using OrderInfrastructure.Helpers;
using OrderInfrastructure.Repository;

namespace OrderInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Adding MongoSettings object to IOC container by filling MongoSettings at "appsettings.json" file
            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
            services.AddTransient<IMongoService, MongoService>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            //Adding HttpRequestSettings object to IOC container
            services.Configure<HttpRequestSettings>(configuration.GetSection(nameof(HttpRequestSettings)));
            services.AddSingleton<IHttpRequestSettings>(provider =>
                provider.GetRequiredService<IOptions<HttpRequestSettings>>().Value);
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddSingleton<IOrderUpdateHelper,OrderUpdateHelper>();
            services.AddSingleton<IOrderCreateHelper,OrderCreateHelper>();
            services.AddSingleton<IOrderChainCreator, OrderChainCreator>();
            
            return services;
        }
    }
}