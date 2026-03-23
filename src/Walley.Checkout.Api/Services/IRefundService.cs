using Walley.Checkout.Api.Models;

namespace Walley.Checkout.Api.Services;

public interface IRefundService
{
    Task<Refund> ProcessRefundAsync(string orderId, decimal amount, string reason);
}
