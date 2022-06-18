using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderDomain.Entities;
using MongoDB.Driver;
using OrderApplication.Exceptions;
using OrderApplication.Interfaces.Repository;
using OrderApplication.Interfaces.Services;
using OrderDomain.RequestFeatures;
using OrderInfrastructure.Extensions;

namespace OrderInfrastructure.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly IMongoService _mongoService;
        public OrderRepository(IMongoService mongoService)
        {
            _mongoService = mongoService;
        }
        
    
        public async Task CreateAsync(Order newOrder)
        {
            await _mongoService.Orders.InsertOneAsync(newOrder);
        }

       
        public async Task UpdateAsync(Order updatedOrder)
        {
            var result = await _mongoService.Orders.ReplaceOneAsync(o => o.Id == updatedOrder.Id, updatedOrder);
            if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
                throw new OrderNotFoundException(updatedOrder.Id);
        }

   
        public async Task DeleteAsync(string orderId)
        {
            var result = await _mongoService.Orders.DeleteOneAsync(o => o.Id == orderId);
            if(result.DeletedCount  == 0)
                throw new OrderNotFoundException(orderId);
        }

      
        public async Task<IEnumerable<Order>> GetAll(RequestParameters requestParameters)
        {
           return await _mongoService.Orders
                .Search(requestParameters.SearchTerm)
                .CustomSort(requestParameters.OrderBy)
                .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
                .Limit(requestParameters.PageSize)
                .ToListAsync();
        }

   
        public async Task<Order> GetWithId(string orderId)
        {
            var order = await _mongoService.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
                throw new OrderNotFoundException(orderId);
            return order;
        }

        
        public async Task ChangeStatus(string orderId, string newStatus)
        {
            var result = await _mongoService.Orders.UpdateOneAsync(
                o => o.Id == orderId,
                Builders<Order>.Update
                    .Set(o => o.Status, newStatus));

            if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
                throw new OrderNotFoundException(orderId);
        }

        
        public async  Task<IEnumerable<Order>> GetOrdersOfCustomer(string customerId,RequestParameters requestParameters)
        {
           var orders =  await _mongoService.Orders
                .Search(requestParameters.SearchTerm,customerId)
                .CustomSort(requestParameters.OrderBy)
                .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
                .Limit(requestParameters.PageSize)
                .ToListAsync();
            if (!orders.Any())
                throw new OrderNotFoundException();
            return orders;
        }

        public async Task DeleteOrderOfCustomer(string customerId)
        {
            var result = await _mongoService.Orders.DeleteManyAsync(o => o.CustomerId == customerId);
            if (result.DeletedCount == 0)
                throw new OrderNotFoundException();
            
        }

        public async Task UpdateProductRelatedOrders(string productId)
        {
            var product =await _mongoService.Products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new ProductNotFoundException(productId);
            }
            var builder = Builders<Order>.Filter;
            var filter = builder.AnyEq(o => o.ProductIds, productId);
            await _mongoService.Orders.Find(filter).ForEachAsync(async order =>
            {
                order.ProductIds.Remove(productId);
                if (order.ProductIds.Count == 0)
                    await DeleteAsync(order.Id);
                else
                {
                    order.Quantity--;
                    order.Total -= product.Price;
                    await UpdateAsync(order);
                }
                    
            });
        }
    }
}