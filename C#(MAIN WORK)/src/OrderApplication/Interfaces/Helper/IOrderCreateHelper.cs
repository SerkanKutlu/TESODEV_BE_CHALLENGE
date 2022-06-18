using System.Threading.Tasks;
using OrderDomain.Entities;

namespace OrderApplication.Interfaces.Helper
{
    public interface IOrderCreateHelper
    {
        Task CheckCustomer(string customerId);
        Task<Order> SetAddressOfOrder(Order newOrder);
        Task<Order> SetTotalAmount(Order newOrder);
    }
}