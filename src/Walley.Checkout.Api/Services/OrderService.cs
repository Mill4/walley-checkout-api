using Walley.Checkout.Api.Models;

namespace Walley.Checkout.Api.Services;

public class OrderService : IOrderService
{
    private readonly List<Order> _orders = new();

    public OrderService()
    {
        SeedData();
    }

    public Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_orders);
    }

    public Task<Order?> GetOrderByIdAsync(string id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task<Order> CreateOrderAsync(Order order)
    {
        order.Id = $"ORD-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        order.CreatedAt = DateTime.UtcNow;
        order.Status = OrderStatus.Pending;
        order.TotalAmount = order.Lines.Sum(l => l.LineTotal);

        _orders.Add(order);

        return Task.FromResult(order);
    }

    private void SeedData()
    {
        _orders.AddRange(new[]
        {
            new Order
            {
                Id = "ORD-001",
                CustomerName = "Anna Svensson",
                CustomerEmail = "anna.svensson@example.com",
                Currency = "SEK",
                Status = OrderStatus.Completed,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                CompletedAt = DateTime.UtcNow.AddDays(-4),
                Lines = new List<OrderLine>
                {
                    new() { ProductName = "Wireless Headphones", Quantity = 1, UnitPrice = 899.00m },
                    new() { ProductName = "USB-C Cable", Quantity = 2, UnitPrice = 149.00m }
                },
                TotalAmount = 1197.00m
            },
            new Order
            {
                Id = "ORD-002",
                CustomerName = "Erik Johansson",
                CustomerEmail = "erik.j@example.com",
                Currency = "SEK",
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                Lines = new List<OrderLine>
                {
                    new() { ProductName = "Laptop Stand", Quantity = 1, UnitPrice = 549.00m }
                },
                TotalAmount = 549.00m
            },
            new Order
            {
                Id = "ORD-003",
                CustomerName = "Maria Lindqvist",
                CustomerEmail = "maria.l@example.com",
                Currency = "NOK",
                Status = OrderStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Lines = new List<OrderLine>
                {
                    new() { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 1299.00m },
                    new() { ProductName = "Desk Pad", Quantity = 1, UnitPrice = 349.00m },
                    new() { ProductName = "Webcam", Quantity = 1, UnitPrice = 799.00m }
                },
                TotalAmount = 2447.00m
            }
        });
    }
}
