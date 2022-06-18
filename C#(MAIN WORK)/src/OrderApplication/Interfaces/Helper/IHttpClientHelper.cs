using System.Threading.Tasks;
using OrderDomain.ValueObjects;

namespace OrderApplication.Interfaces.Helper
{
    public interface IHttpClientHelper
    {

        
        Task<Address> GetCustomerAddressAsync(string customerId);
        Task ValidateCustomerAsync(string customerId);

    }
}