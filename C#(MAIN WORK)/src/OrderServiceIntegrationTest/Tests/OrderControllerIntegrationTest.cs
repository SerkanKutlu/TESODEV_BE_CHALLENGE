using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using OrderApplication.Models.DTO;
using Newtonsoft.Json;
using OrderDomain.ValueObjects;
using Xunit;

namespace OrderServiceIntegrationTest.Tests
{
    public class OrderControllerIntegrationTest:IClassFixture<CustomWebApplicationFactory>,IClassFixture<Mongo2GoFixture>
    {
         private readonly CustomWebApplicationFactory _factory;
         private readonly Mongo2GoFixture _mongoDb;
         private readonly HttpClient _client;
         public OrderControllerIntegrationTest(CustomWebApplicationFactory factory, Mongo2GoFixture mongoDb)
         {
             _factory = factory;
             _mongoDb = mongoDb;
             _mongoDb.SeedData();
             _client = _factory.InjectMongoDbConfigurationSettings(_mongoDb.ConnectionString).CreateClient();
             _client.BaseAddress = new Uri("https://localhost:5001/");
         }
         [Fact]
         public async Task GetAll_Return200()
         {
             var response = await _client.GetAsync("api/Order");
             Assert.Equal(200,((int)response.StatusCode));
         }

         [Theory]
         [InlineData("627f85b7145de8d19a61ed6e")]
         public async Task GetOrderOfCustomer_ValidCustomerId_Return200(string id)
         {
             var response = await _client.GetAsync($"api/order/customer/{id}");
             Assert.Equal(200,((int)response.StatusCode));
         }
         [Theory]
         [InlineData("127f85b7145de8d19a61ed6e")]
         public async Task GetOrderOfCustomer_InValidCustomerId_Return404(string id)
         {
             var response = await _client.GetAsync($"api/order/customer/{id}");
             Assert.Equal(404,((int)response.StatusCode));
         }
         [Theory]
         [InlineData("127f89c1e56326d94a322579")]
         [InlineData("227f89c1e56326d94a322579")]
         [InlineData("327f89c1e56326d94a322579")]
         public async Task GetById_ValidOrderId_Return200(string id)
         {
             var response = await _client.GetAsync($"api/Order/{id}");
             Assert.Equal(200,((int)response.StatusCode));
         }
         [Theory]
         [InlineData("62779b515453b9a42f50dabc")]
         [InlineData("12779b515453b9a42f50dabc")]
         [InlineData("32779b515453b9a42f50dabc")]
         public async Task GetById_InValidOrderId_Return404(string id)
         {
             var response = await _client.GetAsync($"api/Order/{id}");
             Assert.Equal(404,((int)response.StatusCode));
         }
         [Fact]
         public async Task CreateOrder_ValidBody_Return201()
         {
             var body = new OrderForCreation()
             {
                 CustomerId = "627f85b7145de8d19a61ed6e",
                 ProductIds = new List<string>
                 {
                     "627f89b9e56326d94a322578"
                 },
                 Status = "TestStatus"
             };
             var bodyContent = JsonConvert.SerializeObject(body);
             var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
             var httpContent = new ByteArrayContent(buffer);
             httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
             var response = await _client.PostAsync("api/Order", httpContent);
            
             Assert.Equal(201,(int)response.StatusCode);
         }
         [Fact]
         public async Task CreateOrder_InValidCustomer_Return404()
         {
             var body = new OrderForCreation()
             {
                 CustomerId = "137f89c1e56326d94a322579",
                 ProductIds = new List<string>
                 {
                     "627f89b9e56326d94a322578"
                 },
                 Status = "TestStatus"
             };
             var bodyContent = JsonConvert.SerializeObject(body);
             var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
             var httpContent = new ByteArrayContent(buffer);
             httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
             var response = await _client.PostAsync("api/Order", httpContent);
            
             Assert.Equal(404,(int)response.StatusCode);
         }
         [Fact]
         public async Task UpdateOrder_ValidBody_Return200()
         {
             var body = new OrderForUpdate()
             {
                 Id = "227f89c1e56326d94a322579",
                 ProductIds = new List<string>
                 {
                     "627f89b9e56326d94a322578"
                 },
                 Status = "TestStatusUpdate",
                 Address = new Address
                 {
                     City = "test",
                     AddressLine = "test",
                     CityCode = 1,
                     Country = "test"
                 }
                 
             };
             var bodyContent = JsonConvert.SerializeObject(body);
             var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
             var httpContent = new ByteArrayContent(buffer);
             httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
             var response = await _client.PutAsync("api/Order", httpContent);
            
             Assert.Equal(200,(int)response.StatusCode);
         }
         [Fact]
         public async Task UpdateOrder_InValidId_Return404()
         {
             var body = new OrderForUpdate()
             {
                 Id = "827f89c1e56326d94a322579",
                 ProductIds = new List<string>
                 {
                     "627f89b9e56326d94a322578"
                 },
                 Status = "TestStatusUpdate",
                 Address = new Address
                 {
                     City = "test",
                     AddressLine = "test",
                     CityCode = 1,
                     Country = "test"
                 }
                 
             };
             var bodyContent = JsonConvert.SerializeObject(body);
             var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
             var httpContent = new ByteArrayContent(buffer);
             httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
             var response = await _client.PutAsync("api/Order", httpContent);
            
             Assert.Equal(404,(int)response.StatusCode);
         }

         [Theory]
         [InlineData("127f89c1e56326d94a322579")]
         public async Task DeleteOrder_ValidOrderId_Return200(string id)
         {
             var response = await _client.DeleteAsync($"api/Order/{id}");
             Assert.Equal(200,(int)response.StatusCode);
             
         }
                
         [Theory]
         [InlineData("827f89c1e56326d94a322579")]
         public async Task DeleteOrder_InValidOrderId_Return404(string id)
         {
             var response = await _client.DeleteAsync($"api/Order/{id}");
             Assert.Equal(404,(int)response.StatusCode);
             
         }

         [Theory]
         [InlineData("627f85b7145de8d19a61ed6e")]
         public async Task DeleteOrderOfCustomers_ValidCustomerId_Return200(string id)
         {
             var response = await _client.DeleteAsync($"api/Order/Customer/{id}");
             Assert.Equal(200,(int)response.StatusCode);
         }

         [Theory]
         [InlineData("127f85b7145de8d19a61ed6e")]
         public async Task DeleteOrderOfCustomers_InValidCustomerId_Return404(string id)
         {
             var response = await _client.DeleteAsync($"api/Order/Customer/{id}");
             Assert.Equal(404,(int)response.StatusCode);
         }

    }
}