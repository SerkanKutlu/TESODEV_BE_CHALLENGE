using System.Threading.Tasks;

namespace CustomerApplication.Interfaces.Creators
{
    public interface ICustomerChainCreator
    {
        Task CreateCustomerDeleteChain(string customerId);
    }
}