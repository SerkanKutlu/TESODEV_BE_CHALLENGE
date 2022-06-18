using CustomerApplication.Interfaces.Settings;

namespace CustomerInfrastructure.Settings
{
    public class HttpRequestSettings:IHttpRequestSettings
    {
        public string BaseUrl { get; set; }
        public int Port { get; set; }
    }
}