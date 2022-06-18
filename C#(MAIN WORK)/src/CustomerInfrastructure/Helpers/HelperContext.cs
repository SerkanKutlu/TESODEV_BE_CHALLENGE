using System.Threading.Tasks;
using CustomerApplication.Interfaces.Helpers;

namespace CustomerInfrastructure.Helpers
{
    public class HelperContext
    {
        private IHelperStrategy _strategy;
        public void SetStrategy(IHelperStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task<object> ApplyStrategy(object data)
        {
            return await _strategy.Help(data);
        }
    }
}