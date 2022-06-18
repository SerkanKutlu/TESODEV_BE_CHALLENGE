using CustomerDomain.Entities;
using MongoDB.Driver;

namespace CustomerApplication.Interfaces.Services
{
    public interface IMongoService
    {
        IMongoCollection<Customer> Customers { get; }
    }
}