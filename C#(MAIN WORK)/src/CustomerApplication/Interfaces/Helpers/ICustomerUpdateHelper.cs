using System.Threading.Tasks;
using CustomerDomain.Entities;

namespace CustomerApplication.Interfaces.Helpers
{
    public interface ICustomerUpdateHelper
    {
        Task<Customer> PrepareForUpdate(Customer customerForUpdate);
    }
}