using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderApplication.Interfaces;
using OrderApplication.Interfaces.Helper;
using OrderDomain.Entities;

namespace OrderApplication.ActionFilters
{
    public class CustomerExistAttribute:IAsyncActionFilter
    {
        private readonly IOrderCreateHelper _orderCreateHelper;

        public CustomerExistAttribute(IOrderCreateHelper orderCreateHelper)
        {
            _orderCreateHelper = orderCreateHelper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var customerId = context.ActionArguments["customerId"].ToString();;
            await _orderCreateHelper.CheckCustomer(customerId);
            await next();
        }
    }
}