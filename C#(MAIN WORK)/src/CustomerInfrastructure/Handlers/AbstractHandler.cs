using System.Threading.Tasks;
using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Handlers;

namespace CustomerInfrastructure.Handlers
{
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
            return null;
        }
    }
}