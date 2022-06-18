using System.Threading.Tasks;

namespace CustomerApplication.Interfaces.Helpers
{
    public interface IHttpClientHelper
    {

        Task DeleteOrders(string customerId);

    }
}