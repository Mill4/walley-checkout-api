using Microsoft.AspNetCore.Mvc;
using Walley.Checkout.Api.Models;
using Walley.Checkout.Api.Services;

namespace Walley.Checkout.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IRefundService _refundService;

    public OrdersController(IOrderService orderService, IRefundService refundService)
    {
        _orderService = orderService;
        _refundService = refundService;
    }

    /// <summary>
    /// Get all orders.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Get a specific order by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(string id)
    {
        // Task 4: This error handling needs improvement.
        // Right now any issue results in a generic 500 from the exception middleware.
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    // Task 1: Implement POST /api/orders
    // See README for requirements.

    /// <summary>
    /// Request a refund for an order.
    /// </summary>
    [HttpPost("{orderId}/refunds")]
    public async Task<ActionResult<Refund>> RequestRefund(
        string orderId,
        [FromBody] RefundRequest request)
    {
        var refund = await _refundService.ProcessRefundAsync(
            orderId,
            request.Amount,
            request.Reason);

        if (refund.Status == RefundStatus.Rejected)
        {
            return UnprocessableEntity(refund);
        }

        return Ok(refund);
    }
}

public class RefundRequest
{
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}
