using FluentValidation;
using OrderApplication.Models.DTO;
using OrderDomain.Entities;

namespace OrderApplication.Validation
{
    public class OrderForUpdateValidation:AbstractValidator<OrderForUpdate>
    {
        public OrderForUpdateValidation()
        {
            RuleFor(o => o.Id).NotEmpty().NotNull();
            RuleFor(o => o.Status).NotEmpty().NotNull();
            RuleForEach(o => o.ProductIds).NotEmpty().NotNull();
            RuleFor(o => o.Address).SetValidator(new AddressValidation());
            

        }
    }
}