using FluentValidation;
using OrderApplication.Models.DTO;

namespace OrderApplication.Validation
{
    public class ProductForCreateValidation:AbstractValidator<ProductForCreation>
    {

        public ProductForCreateValidation()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull();
            RuleFor(p => p.Price).NotEmpty().NotNull();
            RuleFor(p => p.ImageUrl).NotEmpty().NotNull();
        }
        
    }
}