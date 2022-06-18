using System;

namespace OrderApplication.Exceptions
{
   public class ProductNotFoundException:Exception
    {

        public ProductNotFoundException(string message)
            :base($"No product with id: {message}")
        {
            
        }
    }
}