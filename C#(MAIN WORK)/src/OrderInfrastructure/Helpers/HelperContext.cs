using System.Threading.Tasks;
using OrderApplication.Interfaces.Helper;

namespace OrderInfrastructure.Helpers
{
    //Strategy Design Pattern Context
    public class HelperContext
    {
        private IHelperStrategy _strategy;

        public HelperContext()
        {
            
        }
        public HelperContext(IHelperStrategy strategy)
        {
            _strategy = strategy;
        }

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