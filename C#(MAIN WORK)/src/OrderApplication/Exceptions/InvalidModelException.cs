using System;

namespace OrderApplication.Exceptions
{
    public class InvalidModelException:Exception
    {
        public InvalidModelException(string message):
            base(message)
        {
            
        }
        
    }
}