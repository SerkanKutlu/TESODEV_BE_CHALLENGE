using System.Threading.Tasks;

namespace CustomerApplication.Interfaces.Handlers
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler); //Chain has two steps. That's why it warns.
        Task<object> Handle(object request);
    }
}