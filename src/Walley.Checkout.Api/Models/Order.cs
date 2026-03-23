namespace Walley.Checkout.Api.Models;

public class Order
{
    public string Id { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public List<OrderLine> Lines { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "SEK";
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled,
    Refunded
}
