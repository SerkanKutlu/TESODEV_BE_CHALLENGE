﻿using System;
using System.Linq;
using System.Reflection;
using CustomerDomain.Entities;
using MongoDB.Driver;

namespace CustomerInfrastructure.Extensions
{
    public static class CustomerExtensions
    {
        public static IFindFluent<Customer,Customer> Search(this IMongoCollection<Customer> customers, string searchTerm)
        {
            var findOptions = new FindOptions()
            {
                Collation = new Collation("en", strength: CollationStrength.Secondary)
            };
            return string.IsNullOrWhiteSpace(searchTerm) ?
                customers.Find(c=>true,findOptions) : 
                customers.Find(c => c.Name.ToLower()
                    .Contains(searchTerm.Trim().ToLower()),findOptions);
            
        }

        public static IFindFluent<Customer, Customer> CustomSort(this IFindFluent<Customer, Customer> customers,
            string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return customers.SortBy(e => e.Name);
            var customerParams = orderBy.Trim().Split(',');
            var propertyInfos = typeof(Customer).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var queryProp = string.Empty;
            var direction = string.Empty;
            foreach (var param in customerParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                direction = param.EndsWith(" desc") ? "descending" : "ascending";
                queryProp += objectProperty.Name+",";
                
            }

            var queryPropList = queryProp.TrimEnd(',', ' ').Split(",");
            var customerQuery = "{";
            foreach (var query in queryPropList)
            {
                customerQuery += direction == "ascending" ? $"{query}:1," : $"{query}:-1,";
            }

            customerQuery = customerQuery.TrimEnd(',', ' ');
            customerQuery += "}";
            if (string.IsNullOrWhiteSpace(customerQuery))
                return customers.SortBy(c=>c.Name);
            return customers.Sort(customerQuery);
        }

    }
}