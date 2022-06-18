using System.Threading.Tasks;
using OrderApplication.Interfaces.Handlers;
using OrderApplication.Interfaces.Helper;
using OrderApplication.Interfaces.Repository;
using OrderInfrastructure.Handlers;

namespace OrderInfrastructure.Observers
{
    //Observer design pattern observer
    public class ProductObserver:IObserver
    {
        private readonly IOrderRepository _orderRepository;
        public ProductObserver(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Update(ISubject subject)
        {
            var productId = ((Subject) subject).ProductId;
           await _orderRepository.UpdateProductRelatedOrders(productId);
        }
    }
}