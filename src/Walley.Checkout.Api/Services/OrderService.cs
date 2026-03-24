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

    public Task<Order> GetOrderByIdAsync(string id)
    {
        if (!id.StartsWith("ORD-"))
        {
            throw new InvalidOrderIdException($"Invalid order ID format: {id}");
        }

        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            throw new OrderNotFoundException($"Order with ID {id} not found");
        }

        return Task.FromResult(order);
    }

    /// <summary>
    /// Creates a new order for given customer name and email and order lines.
    /// The order should only be accepted if:
    /// - Customer name and email are provided
    /// - Order lines is not empty
    /// - The quantity of each order line is greater than 0 and unit price is not negative
    /// </summary>
    public Task<Order> CreateOrderAsync(string customerName, string customerEmail, List<OrderLine> orderLines)
    {
        var order = new Order
        {
            Id = $"ORD-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            Lines = orderLines,
            TotalAmount = orderLines.Sum(l => l.LineTotal),
        };

        if (string.IsNullOrWhiteSpace(order.CustomerEmail) || string.IsNullOrWhiteSpace(order.CustomerName))
        {
            order.Status = OrderStatus.Rejected;
            return Task.FromResult(order);
        }

        if (order.Lines == null || order.Lines.Count == 0)
        {
            order.Status = OrderStatus.Rejected;
            return Task.FromResult(order);
        }

        if (order.Lines.Exists(x => x.Quantity <= 0 || x.UnitPrice < 0))
        {
            order.Status = OrderStatus.Rejected;
            return Task.FromResult(order);
        }

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
