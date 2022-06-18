using System.Collections.Generic;
using System.Threading.Tasks;
using OrderDomain.Entities;
using OrderDomain.RequestFeatures;

namespace OrderApplication.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order newOrder);
        Task UpdateAsync(Order updatedOrder);
        Task DeleteAsync(string orderId);
        Task<IEnumerable<Order>> GetAll(RequestParameters orderParameters);
        Task<Order> GetWithId(string orderId);
        Task ChangeStatus(string orderId, string newStatus);
        Task<IEnumerable<Order>> GetOrdersOfCustomer(string customerId,RequestParameters requestParameters);
        Task DeleteOrderOfCustomer(string customerId);
        Task UpdateProductRelatedOrders(string productId);
    }
}