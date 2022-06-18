using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using CustomerDomain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerDomain.Entities
{
    //Validations are not here not to be against to SRP.
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}