using System;

namespace CustomerApplication.Exceptions
{
    public class UniqueEmailAddressException:Exception
    {
        public UniqueEmailAddressException()
        :base("This email already exist")
        {
            
        }
        
    }
}