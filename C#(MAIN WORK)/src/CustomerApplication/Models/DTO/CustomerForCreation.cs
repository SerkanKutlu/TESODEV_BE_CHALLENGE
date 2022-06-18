using CustomerDomain.ValueObjects;

namespace CustomerApplication.Models.DTO
{
    public class CustomerForCreation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}