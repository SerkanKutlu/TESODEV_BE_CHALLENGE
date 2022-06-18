using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerDomain.Entities;
using OrderDomain.RequestFeatures;

namespace CustomerApplication.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task CreateAsync(Customer newCustomer);
        Task UpdateAsync(Customer updatedCustomer);
        Task DeleteAsync(string customerId);
        Task<IEnumerable<Customer>> GetAll(RequestParameters requestParameters);
        Task<Customer> GetWithId(string customerId);
        Task Validate(string customerId);
    }
}