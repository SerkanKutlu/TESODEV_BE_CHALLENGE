using System.Threading.Tasks;

namespace CustomerApplication.Interfaces.Helpers
{
    public interface IHelperStrategy
    {
        Task<object> Help(object data);
    }
}