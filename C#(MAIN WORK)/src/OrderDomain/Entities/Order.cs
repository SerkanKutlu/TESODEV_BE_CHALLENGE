using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderDomain.ValueObjects;

namespace OrderDomain.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public int Quantity { get; set; } //This will be set auto
        public double Total { get; set; } //This will be set auto
        public string Status { get; set; }
        public Address Address { get; set; }
        public List<string> ProductIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}