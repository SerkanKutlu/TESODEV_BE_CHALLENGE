using System.Threading.Tasks;
using OrderApplication.Interfaces.Handlers;

namespace OrderInfrastructure.Handlers
{
    
    //COR base handler
    public abstract class AbstractHandler:IHandler
    {
        private IHandler _nextHandler;
        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

       

        public virtual async Task<object> Handle(object request)
        {
            if (_nextHandler != null)
            {
                return await _nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}