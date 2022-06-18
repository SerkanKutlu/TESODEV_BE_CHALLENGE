using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CustomerApplication.Models.DTO;
using CustomerDomain.ValueObjects;
using Newtonsoft.Json;
using Xunit;

namespace CustomerServiceIntegrationTest.Tests
{
    public class CustomerControllerIntegrationTest:IClassFixture<CustomWebApplicationFactory>,IClassFixture<Mongo2GoFixture>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly Mongo2GoFixture _mongoDb;
        private readonly HttpClient _client;

        public CustomerControllerIntegrationTest(CustomWebApplicationFactory factory, Mongo2GoFixture mongoDb)
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
            var response = await _client.GetAsync("api/Customer");
            Assert.Equal(200,((int)response.StatusCode));
        }

        [Theory]
        [InlineData("627f85b7145de8d19a61ed6e")]
        [InlineData("54651022bffebc03098b4562")]
        [InlineData("54651022bffebc03098b4563")]
        public async Task GetById_ValidCustomerId_Return200(string id)
        {
            var response = await _client.GetAsync($"api/Customer/{id}");
            Assert.Equal(200,((int)response.StatusCode));
        }
        [Theory]
        [InlineData("62779b515453b9a42f50dabc")]
        [InlineData("12779b515453b9a42f50dabc")]
        [InlineData("32779b515453b9a42f50dabc")]
        public async Task GetById_InValidCustomerId_Return404(string id)
        {
            var response = await _client.GetAsync($"api/Customer/{id}");
            Assert.Equal(404,((int)response.StatusCode));
        }
        [Theory]
        [InlineData("627f85b7145de8d19a61ed6e")]
        [InlineData("54651022bffebc03098b4562")]
        [InlineData("54651022bffebc03098b4563")]
        public async Task ValidateCustomer_ValidCustomerId_Return200(string id)
        {
            var response = await _client.GetAsync($"api/Customer/validate/{id}");
            Assert.Equal(200,((int)response.StatusCode));
        }
        [Theory]
        [InlineData("62779b515453b9a42f50dabc")]
        [InlineData("12779b515453b9a42f50dabc")]
        [InlineData("32779b515453b9a42f50dabc")]
        public async Task ValidateCustomer_InValidCustomerId_Return404(string id)
        {
            var response = await _client.GetAsync($"api/Customer/validate/{id}");
            Assert.Equal(404,((int)response.StatusCode));
        }

        [Fact]
        public async Task CreateCustomer_ValidBody_Return201()
        {
            var body = new CustomerForCreation
            {
                Name = "TestName",
                Address = new Address
                {
                    City = "TestCity",
                    AddressLine = "TestLine",
                    CityCode = 1,
                    Country = "TestCountry"
                },
                Email = "test@gmail.com"
            };
            var bodyContent = JsonConvert.SerializeObject(body);
            var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
            var httpContent = new ByteArrayContent(buffer);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("api/Customer", httpContent);
            
            Assert.Equal(201,(int)response.StatusCode);
        }
       
        [Fact]
        public async Task CreateCustomer_InValidBody_Return400()
        {
            var body = new CustomerForCreation
            {
                Name = "TestName",
                Address = new Address
                {
                    City = "TestCity",
                    AddressLine = "TestLine",
                    CityCode = 10,
                    Country = "TestCountry"
                },
                Email = "testgmail"
            };
            var bodyContent = JsonConvert.SerializeObject(body);
            var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
            var httpContent = new ByteArrayContent(buffer);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("api/Customer", httpContent);
            
            Assert.Equal(400,(int)response.StatusCode);
        }
        [Fact]
        public async Task UpdateCustomer_InValidId_Return404()
        {
            var body = new CustomerForUpdate
            {
                Id = "54651022bffebc03098a4563",
                Name = "TestName",
                Address = new Address
                {
                    City = "TestCity",
                    AddressLine = "TestLine",
                    CityCode = 10,
                    Country = "TestCountry"
                },
                Email = "test@gmail"
            };
            var bodyContent = JsonConvert.SerializeObject(body);
            var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
            var httpContent = new ByteArrayContent(buffer);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PutAsync("api/Customer", httpContent);
            
            Assert.Equal(404,(int)response.StatusCode);
        }
        [Fact]
        public async Task UpdateCustomer_ValidId_Return200()
        {
            var body = new CustomerForUpdate
            {
                Id = "54651022bffebc03098b4563",
                Name = "TestName",
                Address = new Address
                {
                    City = "TestCity",
                    AddressLine = "TestLine",
                    CityCode = 10,
                    Country = "TestCountry"
                },
                Email = "test@gmail"
            };
            var bodyContent = JsonConvert.SerializeObject(body);
            var buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
            var httpContent = new ByteArrayContent(buffer);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PutAsync("api/Customer", httpContent);
            
            Assert.Equal(200,(int)response.StatusCode);
        }
        
        [Theory]
        [InlineData("62779b515453b9a42f50dabc")]
        [InlineData("12779b515453b9a42f50dabc")]
        [InlineData("32779b515453b9a42f50dabc")]
        public async Task DeleteCustomer_InValidCustomerId_Return404(string id)
        {
            var response = await _client.DeleteAsync($"api/Customer/{id}");
            Assert.Equal(404,((int)response.StatusCode));
        }
        [Theory]
        [InlineData("627f85b7145de8d19a61ed6e")]
        [InlineData("54651022bffebc03098b4562")]
        [InlineData("54651022bffebc03098b4563")]
        public async Task Delete_ValidCustomerId_Return200(string id)
        {
            var response = await _client.DeleteAsync($"api/Customer/{id}");
            Assert.Equal(200,((int)response.StatusCode));
        }
    }
}