using System.Threading.Tasks;
using OrderApplication.Interfaces.Helper;
using OrderDomain.Entities;

namespace OrderInfrastructure.Helpers
{
    public class UpdateStrategy:IHelperStrategy
    {
        private readonly IOrderUpdateHelper _orderUpdateHelper;

        public UpdateStrategy(IOrderUpdateHelper orderUpdateHelper)
        {
            _orderUpdateHelper = orderUpdateHelper;
        }

        public async Task<object> Help(object data)
        {
            var order = await _orderUpdateHelper.PrepareForUpdate((Order) data);
            return await _orderUpdateHelper.SetTotalAmount(order);
        }
    }
}