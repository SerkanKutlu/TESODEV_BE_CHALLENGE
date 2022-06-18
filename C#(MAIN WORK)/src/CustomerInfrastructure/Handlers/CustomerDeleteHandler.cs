using System.Threading.Tasks;
using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Repository;
using CustomerDomain.Entities;

namespace CustomerInfrastructure.Handlers
{
    public class CustomerDeleteHandler:AbstractHandler
    {
        private readonly ICustomerRepository _customerRepository;


        public CustomerDeleteHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<object> Handle(object request)
        {
            await _customerRepository.DeleteAsync((string)request);
            return await base.Handle(request);
        }
        
    }
}