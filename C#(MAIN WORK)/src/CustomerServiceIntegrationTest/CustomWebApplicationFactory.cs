using CustomerInfrastructure.Settings;
using CustomerPresentation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomerServiceIntegrationTest
{
    public class CustomWebApplicationFactory:WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                config.AddConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build());
            });
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    x => x.UseStartup<Startup>().UseTestServer()
                ).UseEnvironment("Development");
 
            return builder;
        }
        
        public WebApplicationFactory<Startup> InjectMongoDbConfigurationSettings(string connectionString)
        {
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.Configure<MongoSettings>(opts =>
                    {
                        opts.ConnectionString = connectionString;
                    });
 
                });
            });
        }
    }
}