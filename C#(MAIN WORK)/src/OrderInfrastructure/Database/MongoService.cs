using MongoDB.Driver;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Services;
using OrderApplication.Interfaces.Settings;
using OrderDomain.Entities;

namespace OrderInfrastructure.Database
{
    public class MongoService:IMongoService
    {
        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<Product> Products { get; }

        public MongoService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.DatabaseName);
            Orders = mongoDatabase.GetCollection<Order>(mongoSettings.CollectionNames[nameof(Order)]);
            Products = mongoDatabase.GetCollection<Product>(mongoSettings.CollectionNames[nameof(Product)]);
            
        }
    }
}