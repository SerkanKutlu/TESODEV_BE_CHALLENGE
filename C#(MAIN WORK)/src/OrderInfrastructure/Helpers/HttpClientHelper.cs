using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces.Helper;
using OrderApplication.Interfaces.Settings;
using OrderApplication.Models;
using OrderDomain.ValueObjects;

namespace OrderInfrastructure.Helpers
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
       
        public async Task<Address> GetCustomerAddressAsync(string customerId)
        {
            
            var response = await _factory.CreateClient("httpClient").GetAsync($"{_httpRequestSettings.BaseUrl}:{_httpRequestSettings.Port}/Customer/{customerId}");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var customer = await response.Content.ReadFromJsonAsync<CustomerModel>();
                return customer?.Address;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new CustomerNotFoundException(customerId);
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new FormatException();
            }
            

            throw new ServerNotRespondingException();

        }
        public async Task ValidateCustomerAsync(string customerId)
        {
             var response = await _factory.CreateClient("httpClient").GetAsync($"{_httpRequestSettings.BaseUrl}:{_httpRequestSettings.Port}/Customer/validate/{customerId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new CustomerNotFoundException(customerId);
            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new InvalidModelException($"Input format is not valid");
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServerNotRespondingException();


        }
    }
}