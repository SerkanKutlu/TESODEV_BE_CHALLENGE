using MongoDB.Driver;
using OrderDomain.Entities;

namespace OrderApplication.Interfaces.Services
{
    public interface IMongoService
    {
        IMongoCollection<Order> Orders { get; }
        IMongoCollection<Product> Products { get;}
    }
}