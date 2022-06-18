using System.Threading.Tasks;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Helper;
using OrderDomain.Entities;

namespace OrderInfrastructure.Handlers
{
    //COR pattern handler
    public class CustomerControlHandler:AbstractHandler
    {
        private readonly IOrderCreateHelper _orderCreateHelper;

        public CustomerControlHandler(IOrderCreateHelper orderCreateHelper)
        {
            _orderCreateHelper = orderCreateHelper;
        }

        public override async Task<object> Handle(object request)
        {
            await _orderCreateHelper.CheckCustomer(((request as Order)!).CustomerId);
            return await base.Handle(request);
        }
        
    }
}