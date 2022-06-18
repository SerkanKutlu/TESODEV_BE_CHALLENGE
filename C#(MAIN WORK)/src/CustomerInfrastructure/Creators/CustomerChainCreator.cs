using System.Threading.Tasks;
using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Creators;
using CustomerApplication.Interfaces.Helpers;
using CustomerApplication.Interfaces.Repository;
using CustomerDomain.Entities;
using CustomerInfrastructure.Handlers;

namespace CustomerInfrastructure.Creators
{
    public class CustomerChainCreator:ICustomerChainCreator
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpClientHelper _httpClientHelper;

        public CustomerChainCreator(ICustomerRepository customerRepository, IHttpClientHelper httpClientHelper)
        {
            _customerRepository = customerRepository;
            _httpClientHelper = httpClientHelper;
        }

        public async Task CreateCustomerDeleteChain(string customerId)
        {
            var customerDeleteHandler = new CustomerDeleteHandler(_customerRepository);
            var orderDeleteHandler = new OrderDeleteHandler(_httpClientHelper);
            orderDeleteHandler.SetNext(customerDeleteHandler);
            await orderDeleteHandler.Handle(customerId);
        }
    }
}