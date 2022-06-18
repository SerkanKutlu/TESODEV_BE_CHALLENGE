using System.Threading.Tasks;

namespace OrderApplication.Interfaces.Handlers
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        Task Notify();
    }
}