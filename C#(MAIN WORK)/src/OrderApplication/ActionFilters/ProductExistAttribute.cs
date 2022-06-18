using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Repository;
using OrderApplication.Models.DTO;
using OrderDomain.Entities;

namespace OrderApplication.ActionFilters
{
    public class ProductExistAttribute:IAsyncActionFilter
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        
        public ProductExistAttribute(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var order = _mapper.Map<Order>(context.ActionArguments["newOrder"]);
            foreach (var productId in order.ProductIds)
            {
                if (await _productRepository.GetWithId(productId) == null)
                {
                    throw new ProductNotFoundException(productId);
                }
            }
            await next();
        }
    }
}