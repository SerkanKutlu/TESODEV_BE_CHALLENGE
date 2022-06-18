using System;

namespace CustomerApplication.Exceptions
{
    public class InvalidModelException:Exception
    {
        public InvalidModelException(string message):
            base(message)
        {
            
        }
        
    }
}