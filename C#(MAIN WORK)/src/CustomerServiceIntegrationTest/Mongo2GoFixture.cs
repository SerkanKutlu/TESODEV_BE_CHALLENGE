using System;
using CustomerDomain.Entities;
using Mongo2Go;
using MongoDB.Driver;

namespace CustomerServiceIntegrationTest
{
    public class Mongo2GoFixture:IDisposable
    {
        public MongoClient Client { get; }
 
        public IMongoDatabase Database { get; }
 
        public string ConnectionString { get; }
 
        private readonly MongoDbRunner _mongoRunner;
 
        private readonly string _databaseName = "CustomerServiceDb";
 
        public IMongoCollection<Customer> DataBoundCollection { get; }
        
        public Mongo2GoFixture()
        {
            // initializes the instance
            _mongoRunner = MongoDbRunner.Start();
 
            // store the connection string with the chosen port number
            ConnectionString = _mongoRunner.ConnectionString;
 
            // create a client and database for use outside the class
            Client = new MongoClient(ConnectionString);
 
            Database = Client.GetDatabase(_databaseName);
 
            // initialize your collection
            DataBoundCollection = Database.GetCollection<Customer>("Customers");
        }

        public void SeedData()
        {
            var documentCount = DataBoundCollection.CountDocuments(Builders<Customer>.Filter.Empty);
            if (documentCount==0)
            {
                _mongoRunner.Import(_databaseName,"Customers", "seedCustomerData.json",false);
                
            }
        }
        
        public void Dispose()
        {
            _mongoRunner.Dispose();
        }
    }
}