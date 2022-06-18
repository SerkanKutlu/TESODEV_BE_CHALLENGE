using System.Threading.Tasks;

namespace OrderApplication.Interfaces.Helper
{
    public interface IHelperStrategy
    {
        Task<object> Help(object data);
    }
}