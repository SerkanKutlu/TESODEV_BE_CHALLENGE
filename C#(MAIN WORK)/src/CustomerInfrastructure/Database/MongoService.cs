using CustomerApplication.Interfaces.Services;
using CustomerApplication.Interfaces.Settings;
using CustomerDomain.Entities;
using MongoDB.Driver;

namespace CustomerInfrastructure.Database
{
    public class MongoService:IMongoService
    {
        public IMongoCollection<Customer> Customers { get; }


        public MongoService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.DatabaseName);
            Customers = mongoDatabase.GetCollection<Customer>(mongoSettings.CollectionName);
            var options = new CreateIndexOptions {Unique = true};
            var indexModel = new CreateIndexModel<Customer>("{Email:1}",options);
            Customers.Indexes.CreateOne(indexModel);
            
        }
    }
}