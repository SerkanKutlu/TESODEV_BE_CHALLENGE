using System.Collections.Generic;
using OrderDomain.Entities;
using OrderDomain.ValueObjects;

namespace OrderApplication.Models.DTO
{
    public class OrderForUpdate
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public Address Address { get; set; }
        public List<string> ProductIds { get; set; }
    }
}