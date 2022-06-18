
using System.Threading.Tasks;
using AutoMapper;
using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Creators;
using CustomerApplication.Interfaces.Helpers;
using CustomerApplication.Interfaces.Repository;
using CustomerApplication.Models.DTO;
using CustomerDomain.Entities;
using CustomerInfrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderDomain.RequestFeatures;


namespace CustomerPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerUpdateHelper _customerUpdateHelper;
        private readonly ICustomerChainCreator _customerChainCreator;
        public CustomerController(IMapper mapper, ILogger<CustomerController> logger, ICustomerRepository customerRepository, ICustomerUpdateHelper customerUpdateHelper, ICustomerChainCreator customerChainCreator)
        {
            _mapper = mapper;
            _logger = logger;
            _customerRepository = customerRepository;
            _customerUpdateHelper = customerUpdateHelper;
            _customerChainCreator = customerChainCreator;
        }
        
        #region Get Requests
        /// <summary>
        /// List all customers
        /// </summary>
        /// <param name="requestParameters">Paging params if needed</param>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
        {
            var customers =await _customerRepository.GetAll(requestParameters);
            _logger.LogInformation("Getting all customers data's from database");
            return Ok(customers);
        }

        /// <summary>
        /// Fetch a customer with giving id
        /// </summary>
        ///<param name="id">Id of specific customer</param>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Customer Id</response>
        /// <response code="400">Invalid Id Format Error</response>
        /// <response code="500">Server Error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var customer = await _customerRepository.GetWithId(id);
            _logger.LogInformation($"Getting customer data with {id} from database");
            return Ok(customer);
        }
        /// <summary>
        /// Check the if customer exist with an id.
        /// </summary>
        /// <remarks>
        /// This endpoint can be used by less authorized users.
        /// </remarks>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Customer Id</response>
        /// <response code="400">Invalid Id Format Error</response>
        /// <response code="500">Server Error</response>
        ///<param name="id">Id of specific customer</param>
        [HttpGet("validate/{id}")]
        public async Task<IActionResult> ValidateCustomer(string id)
        {
            await _customerRepository.Validate(id);
            _logger.LogInformation($"Validated customer with {id} from database");
            return Ok();
        }
        
        #endregion
        #region Post Requests

        /// <summary>
        /// Creating new customer by using body.
        /// </summary>
        /// <response code="201">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <param name="customerForCreation">All areas are required</param>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerForCreation customerForCreation)
        {
            var customer = _mapper.Map<Customer>(customerForCreation);
            await _customerRepository.CreateAsync(customer);
            _logger.LogInformation($"New customer added with id {customer.Id}");
            return CreatedAtAction("GetById", new {id = customer.Id}, customer);
        }
        #endregion
        #region Put Requests
        /// <summary>
        /// Updating a customer by using body.
        /// </summary>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="404">Invalid Customer Id</response>
        /// <response code="500">Server Error</response>
        /// <param name="customerForUpdate">All areas are required. Enter correct pattern and valid id.</param>
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerForUpdate customerForUpdate)
        {
            var customer = _mapper.Map<Customer>(customerForUpdate);
            
            //Strategy design pattern usage (It seems unnecessary but can be useful for future features)
            //For example, updated customer can be used for some other logging areas like
            //which area updated mostly by customers. At that time, creating new strategy
            //named UpdatedAreaLogStrategy and just setting context with it will be enough.
            
            //It is more useful for OrderService(Using at the COR design pattern handler).
            
            //HelperContext object can be used as different objects thanks to Strategy design pattern.
            var helperContext = new HelperContext();
            helperContext.SetStrategy(new UpdateStrategy(_customerUpdateHelper));
            customer = (Customer)await helperContext.ApplyStrategy(customer);
            await _customerRepository.UpdateAsync(customer);
            _logger.LogInformation($"Customer with id {customer.Id} updated.");
            return Ok(customer);
        }
        #endregion
        #region Delete Requests
        /// <summary>
        /// Deleting a customer by using id of the customer.
        /// </summary>
        /// <param name="id">Enter a valid id of a customer</param>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Customer Id</response>
        /// <response code="503">Error With Remote Server</response>
        /// <response code="500">Server Error</response>
        /// <response code="400">Invalid Id Format Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            //Here, there is a deleting chain. When a customer deleted,
            //related orders should be deleted, too. So, COR design pattern
            //makes sense here.
            
            await _customerChainCreator.CreateCustomerDeleteChain(id);
            _logger.LogInformation($"Customer with id {id} deleted.");
            return Ok();//May be returned NoContent() 204
        }
        #endregion
    }
}