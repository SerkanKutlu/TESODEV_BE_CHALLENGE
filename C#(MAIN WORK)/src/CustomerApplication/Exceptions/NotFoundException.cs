using System;

namespace CustomerApplication.Exceptions
{
    public class NotFoundException:Exception
    {

        public NotFoundException(string message)
        :base($"No customer with id: {message}")
        {
            
        }
    }
}