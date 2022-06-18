using System.Threading.Tasks;
using CustomerApplication.Interfaces.Helpers;
using CustomerDomain.Entities;

namespace CustomerInfrastructure.Helpers
{
    public class UpdateStrategy:IHelperStrategy
    {
        private readonly ICustomerUpdateHelper _customerUpdateHelper;

        public UpdateStrategy(ICustomerUpdateHelper customerUpdateHelper)
        {
            _customerUpdateHelper = customerUpdateHelper;
        }

        public async Task<object> Help(object data)
        {
            return await _customerUpdateHelper.PrepareForUpdate((Customer) data);
        }
    }
}