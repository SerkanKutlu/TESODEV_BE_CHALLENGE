using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CustomerApplication.Exceptions;
using CustomerApplication.Interfaces.Helpers;
using CustomerApplication.Interfaces.Settings;

namespace CustomerInfrastructure.Helpers
{
    public class HttpClientHelper:IHttpClientHelper
    {
        private readonly IHttpRequestSettings _httpRequestSettings;
        private readonly IHttpClientFactory _factory;
        
        public HttpClientHelper(IHttpRequestSettings httpRequestSettings, IHttpClientFactory factory)
        {
            _httpRequestSettings = httpRequestSettings;
            _factory = factory;
        }
        
        public async Task DeleteOrders(string customerId)
        {
            //Client has a name because it has a retry policy.
            var result =
                await _factory.CreateClient("httpClient").DeleteAsync(
                    $"{_httpRequestSettings.BaseUrl}:{_httpRequestSettings.Port}/Order/customer/{customerId}");
            if (result.StatusCode!=HttpStatusCode.NotFound && result.StatusCode!= HttpStatusCode.OK )
            {
                //If there is no order of the customer, no action needed.
                throw new ServerNotRespondingException();
            }
        }
    }
}