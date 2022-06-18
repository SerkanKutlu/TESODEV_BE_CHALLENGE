using System.Collections.Generic;
using System.Threading.Tasks;
using OrderDomain.Entities;
using MongoDB.Driver;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces.Repository;
using OrderApplication.Interfaces.Services;
using OrderDomain.RequestFeatures;
using OrderInfrastructure.Extensions;

namespace OrderInfrastructure.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly IMongoService _mongoService;

        public ProductRepository(IMongoService mongoService)
        {
            _mongoService = mongoService;
        }

   
        public async Task CreateAsync(Product newProduct)
        {
            await _mongoService.Products.InsertOneAsync(newProduct);
        }

    
        public async Task UpdateAsync(Product updatedProduct)
        {
            var result = await _mongoService.Products.ReplaceOneAsync(p => p.Id == updatedProduct.Id, updatedProduct);
            if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
                throw new ProductNotFoundException(updatedProduct.Id);
        }
        
        
        public async Task DeleteAsync(string productId)
        { 
            var result = await _mongoService.Products.DeleteOneAsync(p => p.Id == productId);
            if (result.DeletedCount == 0)
                throw new ProductNotFoundException(productId);
        }
     
        public async Task<IEnumerable<Product>> GetAll(RequestParameters requestParameters)
        {
            return await  _mongoService.Products.Search(requestParameters.SearchTerm)
                .CustomSort(requestParameters.OrderBy)
                .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
                .Limit(requestParameters.PageSize)
                .ToListAsync();

        }

        public async Task<Product> GetWithId(string productId)
        {
            var product = await _mongoService.Products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null)
                throw new ProductNotFoundException(productId);
            return product;
        }
    }
}