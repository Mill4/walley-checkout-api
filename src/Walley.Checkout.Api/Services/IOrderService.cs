using Walley.Checkout.Api.Models;

namespace Walley.Checkout.Api.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(string id);
    Task<Order> CreateOrderAsync(Order order);
}
