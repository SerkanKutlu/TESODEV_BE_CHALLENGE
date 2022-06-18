using System;
using Mongo2Go;
using MongoDB.Driver;
using OrderDomain.Entities;

namespace OrderServiceIntegrationTest
{
    public class Mongo2GoFixture:IDisposable
    {
        public MongoClient Client { get; }
 
        public IMongoDatabase Database { get; }
 
        public string ConnectionString { get; }
 
        private readonly MongoDbRunner _mongoRunner;
 
        private readonly string _databaseName = "OrderServiceDb";
 
        public IMongoCollection<Order> DataBoundCollection { get; }
        
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
            DataBoundCollection = Database.GetCollection<Order>("Orders");
        }

        public void SeedData()
        {
            var documentCount = DataBoundCollection.CountDocuments(Builders<Order>.Filter.Empty);
            if (documentCount==0)
            {
                _mongoRunner.Import(_databaseName,"Orders", "seedOrderData.json",false);
                _mongoRunner.Import(_databaseName,"Products","seedProductData.json",false);
            }
        }
        
        public void Dispose()
        {
            _mongoRunner.Dispose();
        }
    }
}