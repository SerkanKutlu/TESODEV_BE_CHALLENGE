using System.Threading.Tasks;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Helper;
using OrderApplication.Interfaces.Repository;
using OrderDomain.Entities;

namespace OrderInfrastructure.Helpers
{
    public class OrderCreateHelper:IOrderCreateHelper
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IProductRepository _productRepository;
        public OrderCreateHelper(IHttpClientHelper httpClientHelper, IProductRepository productRepository)
        {
            _httpClientHelper = httpClientHelper;
            _productRepository = productRepository;
        }

        public async Task CheckCustomer(string customerId)
        {
            await _httpClientHelper.ValidateCustomerAsync(customerId);
        }

        public async Task<Order> SetAddressOfOrder(Order newOrder)
        {
            var address = await _httpClientHelper.GetCustomerAddressAsync(newOrder.CustomerId);
            newOrder.Address = address;
            return newOrder;
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