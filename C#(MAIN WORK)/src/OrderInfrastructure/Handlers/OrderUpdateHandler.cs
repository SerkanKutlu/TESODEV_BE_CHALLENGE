using System.Threading.Tasks;
using OrderApplication.Interfaces.Helper;
using OrderDomain.Entities;
using OrderInfrastructure.Helpers;

namespace OrderInfrastructure.Handlers
{
    public class OrderUpdateHandler:AbstractHandler
    {
        private readonly IOrderUpdateHelper _orderUpdateHelper;

        public OrderUpdateHandler(IOrderUpdateHelper orderUpdateHelper)
        {
            _orderUpdateHelper = orderUpdateHelper;
        }
        
        
      

        public override async Task<object> Handle(object request)
        {
            var helperContext = new HelperContext();
            helperContext.SetStrategy(new UpdateStrategy(_orderUpdateHelper));
            return await helperContext.ApplyStrategy(request);
        }
    }
}