using System.Threading.Tasks;
using OrderDomain.Entities;

namespace OrderApplication.Interfaces.Helper
{
    public interface IOrderUpdateHelper
    {
        Task<Order> PrepareForUpdate(Order orderForUpdate);
        Task<Order> SetTotalAmount(Order newOrder);
    }
}