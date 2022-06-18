using System;

namespace CustomerApplication.Exceptions
{
    public class ServerNotRespondingException:Exception
    {
        public ServerNotRespondingException()
        :base("Remote service is not responding, try again later")
        {
            
        }
        public ServerNotRespondingException(string message)
            :base(message)
        {
            
        }
        
        
    }
}