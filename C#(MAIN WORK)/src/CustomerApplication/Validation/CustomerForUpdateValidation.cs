using System.Data;
using CustomerApplication.Models.DTO;
using FluentValidation;

namespace CustomerApplication.Validation
{
    public class CustomerForUpdateValidation:AbstractValidator<CustomerForUpdate>
    {
        public CustomerForUpdateValidation()
        {
            RuleFor(c => c.Id).NotEmpty().NotNull();
            RuleFor(c => c.Name).NotEmpty().NotNull().MaximumLength(20).WithMessage("Name can not be longer than 20 characters");
            RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Enter a valid email address");
            RuleFor(c => c.Address).SetValidator(new AddressValidation());
        }
    }
}