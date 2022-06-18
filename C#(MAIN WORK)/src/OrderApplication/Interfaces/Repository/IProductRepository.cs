using System.Collections.Generic;
using System.Threading.Tasks;
using OrderDomain.Entities;
using OrderDomain.RequestFeatures;

namespace OrderApplication.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task CreateAsync(Product newProduct);
        Task UpdateAsync(Product updatedProduct);
        Task DeleteAsync(string productId);
        Task<IEnumerable<Product>> GetAll(RequestParameters requestParameters);
        Task<Product> GetWithId(string productId);
    }
}