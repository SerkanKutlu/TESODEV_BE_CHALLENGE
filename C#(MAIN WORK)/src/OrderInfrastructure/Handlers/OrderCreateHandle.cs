using System.Threading.Tasks;
using OrderApplication.Interfaces.Helper;
using OrderInfrastructure.Helpers;

namespace OrderInfrastructure.Handlers
{
    //COR pattern handler
    public class OrderCreateHandle:AbstractHandler
    {
        private readonly IOrderCreateHelper _orderCreateHelper;
        public OrderCreateHandle(IOrderCreateHelper orderCreateHelper)
        {
            _orderCreateHelper = orderCreateHelper;
        }

        public override async Task<object> Handle(object request)
        {
            //Strategy pattern
            var helperContext = new HelperContext();
            helperContext.SetStrategy(new CreateStrategy(_orderCreateHelper));
            return await helperContext.ApplyStrategy(request);
        }
        
    }
}