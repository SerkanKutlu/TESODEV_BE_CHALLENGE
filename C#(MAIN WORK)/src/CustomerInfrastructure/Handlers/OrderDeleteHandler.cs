using System.Threading.Tasks;
using CustomerApplication.Interfaces;
using CustomerApplication.Interfaces.Helpers;
using CustomerDomain.Entities;

namespace CustomerInfrastructure.Handlers
{
    public class OrderDeleteHandler:AbstractHandler
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public OrderDeleteHandler(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
        
        public override async Task<object> Handle(object request)
        {
            await _httpClientHelper.DeleteOrders((string)request);
            return await base.Handle(request);
        }
    }
}