using System.Threading.Tasks;
using CustomerApplication.Exceptions;
using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Helpers;
using CustomerApplication.Interfaces.Repository;
using CustomerDomain.Entities;

namespace CustomerInfrastructure.Helpers
{
    public class CustomerUpdateHelper:ICustomerUpdateHelper
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerUpdateHelper(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> PrepareForUpdate(Customer customerForUpdate)
        {
            var oldCustomer = await _customerRepository.GetWithId(customerForUpdate.Id);
            if (oldCustomer == null) throw new NotFoundException(customerForUpdate.Id);
            customerForUpdate.CreatedAt = oldCustomer.CreatedAt;
            return customerForUpdate;
        } 
    }
}