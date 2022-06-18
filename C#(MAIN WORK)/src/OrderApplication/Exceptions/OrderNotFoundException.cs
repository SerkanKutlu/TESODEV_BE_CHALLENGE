using System;

namespace OrderApplication.Exceptions
{
    public class OrderNotFoundException:Exception
    {

        public OrderNotFoundException()
            :base("There is no order with the customer")
        {
            
        }
        public OrderNotFoundException(string message)
            :base($"No order with id: {message}")
        {
            
        }
    }
}