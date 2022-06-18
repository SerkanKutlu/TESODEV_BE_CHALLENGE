using System.Threading.Tasks;

namespace OrderApplication.Interfaces.Handlers
{
    //COR base handler interface
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        Task<object> Handle(object request);
    }
}