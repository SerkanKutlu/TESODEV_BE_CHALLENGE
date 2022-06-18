using System;

namespace OrderApplication.Exceptions
{
    public class CustomerNotFoundException:Exception
    {
        public CustomerNotFoundException(string customerId):
            base($"Customer with id:{customerId} is not found")
        {
            
        }
    }
}