using System.Collections.Generic;
using OrderDomain.Entities;
using OrderDomain.ValueObjects;

namespace OrderApplication.Models.DTO
{
    public class OrderForCreation
    {
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public List<string> ProductIds { get; set; }
    }
}