using CustomerApplication.Models.DTO;
using CustomerDomain.Entities;
using FluentValidation;

namespace CustomerApplication.Validation
{
    public class CustomerForCreationValidation:AbstractValidator<CustomerForCreation>
    {

        public CustomerForCreationValidation()
        {
            RuleFor(c => c.Name).NotEmpty().NotNull().MaximumLength(20).WithMessage("Name can not be longer than 20 characters");
            RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Enter a valid email address");
            RuleFor(c => c.Address).SetValidator(new AddressValidation());
        }
        
    }
}