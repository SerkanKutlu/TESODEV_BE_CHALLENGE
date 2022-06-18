using System;
using System.Threading.Tasks;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Repository;
using OrderDomain.Entities;

namespace OrderInfrastructure.Handlers
{
    //COR pattern handler
    public class ProductControlHandler:AbstractHandler
    {
        private readonly IProductRepository _productRepository;

        public ProductControlHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<object> Handle(object request)
        {
            foreach (var productId in ((request as Order)!).ProductIds)
            {
                await _productRepository.GetWithId(productId);
            }

            return await base.Handle(request);
        }
    }
}