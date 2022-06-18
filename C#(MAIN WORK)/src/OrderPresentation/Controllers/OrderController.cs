﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderApplication.ActionFilters;
using OrderApplication.Interfaces.Creators;
using OrderApplication.Interfaces.Repository;
using OrderApplication.Models.DTO;
using OrderDomain.Entities;
using OrderDomain.RequestFeatures;

namespace OrderPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderChainCreator _orderChainCreator;
        public OrderController(IOrderRepository orderRepository, ILogger<OrderController> logger, IMapper mapper, IOrderChainCreator orderChainCreator)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
            _orderChainCreator = orderChainCreator;
  
        }


        #region Get Requests

        /// <summary>
        /// List all order
        /// </summary>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        [HttpGet]
        //[ResponseCache(Duration = 120)]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
        {
            var orders = await _orderRepository.GetAll(requestParameters);
            _logger.LogInformation("Getting all orders data's from database");
            return Ok(orders);
        }
        
        /// <summary>
        /// Fetch a order with giving id
        /// </summary>
        ///<param name="id">Id of specific order</param>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Order Id</response>
        /// <response code="500">Server Error</response>
        /// <response code="400">Invalid Format Error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderRepository.GetWithId(id);
            _logger.LogInformation($"Getting order data with {id} from database");
            return Ok(order);
        }

        /// <summary>
        /// Fetch a orders of the given customer
        /// </summary>
        ///<param name="customerId">Id of specific customer</param>
        ///<param name="requestParameters">Paging params if needed</param>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Customer Id or No Order With The Customer</response>
        /// <response code="400">Invalid Id Format Error</response>
        [HttpGet("customer/{customerId}")]
        [ServiceFilter(typeof(CustomerExistAttribute))]
        public async Task<IActionResult> GetOrdersOfCustomers(string customerId,[FromQuery] RequestParameters requestParameters)
        {
            var orders =await _orderRepository.GetOrdersOfCustomer(customerId,requestParameters);
            _logger.LogInformation($"Orders of customer with id {customerId} fetched");
            return Ok(orders);
        }
        #endregion
        #region Post Requests

        /// <summary>
        /// Creating new order by using body.
        /// </summary>
        /// <param name="newOrder">All areas are required</param>
        /// <response code="201">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Customer or Product Id</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderForCreation newOrder)
        {
            var order = _mapper.Map<Order>(newOrder);
            await _orderChainCreator.CreateChainForOrderCreation(order);
            await _orderRepository.CreateAsync(order);
            _logger.LogInformation($"New order added with id {order.Id}");
            return CreatedAtAction("GetById", new {id = order.Id}, order);
        }
        #endregion
        #region Put Requests
        /// <summary>
        /// Updating an order by using body.
        /// </summary>
        /// <param name="newOrder">All areas are required. Enter correct pattern and valid id.</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Order or Product Id</response>
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderForUpdate newOrder)
        {
            var order = _mapper.Map<Order>(newOrder);
            order =await _orderChainCreator.CreateChainForOrderUpdate(order);
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation($"Order with id {order.Id} updated.");
            return Ok(order);
            
        }

        /// <summary>
        /// Updating the status of an order by using body.
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <param name="newStatus">New status of the order</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid OrderId</response>
        [HttpPut("status/{id}/{newStatus}")]
        public async Task<IActionResult> UpdateStatus(string id, string newStatus)
        {
            await _orderRepository.ChangeStatus(id, newStatus);
            return Ok();
        }
        #endregion
        #region Delete Requests
        /// <summary>
        /// Deleting a order by using id of the order.
        /// </summary>
        /// <param name="id">Enter a valid id of a order</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Invalid Id Format Error</response>
        /// <response code="404">Invalid Order Id </response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderRepository.DeleteAsync(id);
            _logger.LogInformation($"Order with id {id} deleted.");
            return Ok();//May be returned NoContent() 204
        }

        /// <summary>
        /// Deleting all orders of a customer with given id.
        /// </summary>
        /// <param name="customerId">Enter a valid id of a customer</param>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Customer Id or No Order With The Customer</response>
        /// <response code="400">Invalid Id Format Error</response>
        [HttpDelete("customer/{customerId}")]
        [ServiceFilter(typeof(CustomerExistAttribute))]
        public async Task<IActionResult> DeleteOrderOfCustomer(string customerId)
        {
            await _orderRepository.DeleteOrderOfCustomer(customerId);
            _logger.LogInformation($"Orders of customer with customer id {customerId} deleted.");
            return Ok();
        }
        #endregion
    }
}