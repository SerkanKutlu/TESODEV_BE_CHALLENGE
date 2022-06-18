using FluentValidation;
using OrderApplication.Models.DTO;
using OrderDomain.Entities;

namespace OrderApplication.Validation
{
    public class OrderForCreateValidation:AbstractValidator<OrderForCreation>
    {
        public OrderForCreateValidation()
        {
            RuleFor(o=>o.CustomerId).NotEmpty().NotNull();
            RuleFor(o => o.Status).NotEmpty().NotNull();
            RuleForEach(o => o.ProductIds).NotEmpty().NotNull();

        }
    }
}