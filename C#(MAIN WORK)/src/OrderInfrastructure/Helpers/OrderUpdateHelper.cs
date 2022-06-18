using System.Threading.Tasks;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Helper;
using OrderApplication.Interfaces.Repository;
using OrderDomain.Entities;

namespace OrderInfrastructure.Helpers
{
    public class OrderUpdateHelper:IOrderUpdateHelper
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public OrderUpdateHelper(IOrderRepository orderRepository,IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        
        public async Task<Order> PrepareForUpdate(Order orderForUpdate)
        {
            var oldOrder =await _orderRepository.GetWithId(orderForUpdate.Id);
            if (oldOrder == null)
                throw new OrderNotFoundException(orderForUpdate.Id);
            orderForUpdate.CreatedAt = oldOrder.CreatedAt;
            orderForUpdate.CustomerId = oldOrder.CustomerId;
            return orderForUpdate;
        }

        public async Task<Order> SetTotalAmount(Order newOrder)
        {
            var total = 0d;
            foreach (var id in newOrder.ProductIds)
            {
                var product = await _productRepository.GetWithId(id);
                total += product.Price;
            }

            newOrder.Total = total;
            return newOrder;
        }
    }
}