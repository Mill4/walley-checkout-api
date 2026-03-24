using Walley.Checkout.Api.Models;

namespace Walley.Checkout.Api.Services;

public class RefundService : IRefundService
{
    private readonly IOrderService _orderService;

    public RefundService(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Processes a refund for the given order.
    /// The refund should only be approved if:
    /// - The order exists
    /// - The order status is Completed
    /// - The refund amount does not exceed the order total
    /// </summary>
    public async Task<Refund> ProcessRefundAsync(string orderId, decimal amount, string reason)
    {
        // await the order from the order service, Task is never null here    
        var order = await _orderService.GetOrderByIdAsync(orderId);

        var refund = new Refund
        {
            Id = $"REF-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
            OrderId = orderId,
            Amount = amount,
            Reason = reason
        };

        if (order == null)
        {
            refund.Status = RefundStatus.Rejected;
            return refund;
        }

        if (order.Status != OrderStatus.Completed)
        {
            refund.Status = RefundStatus.Rejected;
            return refund;
        }

        if (amount > order.TotalAmount)
        {
            refund.Status = RefundStatus.Rejected;
            return refund;
        }

        refund.Status = RefundStatus.Approved;
        return refund;
    }
}
