using System.Threading.Tasks;
using OrderDomain.Entities;

namespace OrderApplication.Interfaces.Creators
{
    public interface IOrderChainCreator
    {
        //COR pattern chain creation interface
        Task<Order> CreateChainForOrderCreation(Order order);
        Task<Order> CreateChainForOrderUpdate(Order order);
    }
}