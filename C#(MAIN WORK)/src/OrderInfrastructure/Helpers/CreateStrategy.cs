using System.Threading.Tasks;
using OrderApplication.Interfaces.Helper;
using OrderDomain.Entities;

namespace OrderInfrastructure.Helpers
{
    public class CreateStrategy:IHelperStrategy
    {
        private readonly IOrderCreateHelper _orderCreateHelper;

        public CreateStrategy(IOrderCreateHelper orderCreateHelper)
        {
            _orderCreateHelper = orderCreateHelper;
            
        }

        public async Task<object> Help(object data)
        {
            var order = await _orderCreateHelper.SetAddressOfOrder((Order)data);
            return await _orderCreateHelper.SetTotalAmount(order);
        }
    }
}