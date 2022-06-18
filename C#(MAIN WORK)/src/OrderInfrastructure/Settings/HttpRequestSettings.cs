using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Settings;

namespace OrderInfrastructure.Helpers
{
    public class HttpRequestSettings:IHttpRequestSettings
    {
        public string BaseUrl { get; set; }
        public int Port { get; set; }
    }
}