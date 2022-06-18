﻿using CustomerDomain.ValueObjects;

namespace CustomerApplication.Models.DTO
{
    public class CustomerForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}