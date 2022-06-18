using System.Collections.Generic;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Settings;

namespace OrderInfrastructure.Database
{
    public class MongoSettings:IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public Dictionary<string, string> CollectionNames { get; set; }
    }
}