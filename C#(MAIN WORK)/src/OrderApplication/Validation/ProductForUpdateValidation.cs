using FluentValidation;
using OrderApplication.Models.DTO;

namespace OrderApplication.Validation
{
    public class ProductForUpdateValidation:AbstractValidator<ProductForUpdate>
    {

        public ProductForUpdateValidation()
        {
            RuleFor(p => p.Id).NotEmpty().NotNull();
            RuleFor(p => p.Name).NotEmpty().NotNull();
            RuleFor(p => p.Price).NotEmpty().NotNull();
            RuleFor(p => p.ImageUrl).NotEmpty().NotNull();
        }
        
    }
}