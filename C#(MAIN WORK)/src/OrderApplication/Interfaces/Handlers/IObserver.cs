using System.Threading.Tasks;

namespace OrderApplication.Interfaces.Handlers
{
    public interface IObserver
    {
        Task Update(ISubject subject);
    }
}