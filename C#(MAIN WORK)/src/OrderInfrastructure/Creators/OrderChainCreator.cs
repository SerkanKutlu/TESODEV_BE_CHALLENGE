using System.Threading.Tasks;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Creators;
using OrderApplication.Interfaces.Helper;
using OrderApplication.Interfaces.Repository;
using OrderDomain.Entities;
using OrderInfrastructure.Handlers;

namespace OrderInfrastructure.Creators
{
    public class OrderChainCreator:IOrderChainCreator
    {
        private readonly IOrderCreateHelper _orderCreateHelper;
        private readonly IOrderUpdateHelper _orderUpdateHelper;
        private readonly IProductRepository _productRepository;

        public OrderChainCreator(IOrderCreateHelper orderCreateHelper, IProductRepository productRepository, IOrderUpdateHelper orderUpdateHelper)
        {
            _orderCreateHelper = orderCreateHelper;
            _productRepository = productRepository;
            _orderUpdateHelper = orderUpdateHelper;
        }

        public async Task<Order> CreateChainForOrderCreation(Order order)
        {
            var productHandler = new ProductControlHandler(_productRepository);
            var customerHandler = new CustomerControlHandler(_orderCreateHelper);
            var orderCreateHandler = new OrderCreateHandle(_orderCreateHelper);
            //Creating a chain
            productHandler.SetNext(customerHandler).SetNext(orderCreateHandler);
            return (Order) await productHandler.Handle(order);
        }

        public async Task<Order> CreateChainForOrderUpdate(Order order)
        {
            //Customer can not be updated. Need to create new order for new customer.
            var productHandler = new ProductControlHandler(_productRepository);
            var orderUpdateHandler = new OrderUpdateHandler(_orderUpdateHelper);
            //Creating a chain
            productHandler.SetNext(orderUpdateHandler);
            return  (Order)await productHandler.Handle(order);
        }
    }
}