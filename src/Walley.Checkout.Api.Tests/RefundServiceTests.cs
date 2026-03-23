using NSubstitute;
using FluentAssertions;
using Xunit;
using Walley.Checkout.Api.Models;
using Walley.Checkout.Api.Services;

namespace Walley.Checkout.Api.Tests;

public class RefundServiceTests
{
    private readonly IOrderService _orderService;
    private readonly RefundService _sut;

    public RefundServiceTests()
    {
        _orderService = Substitute.For<IOrderService>();
        _sut = new RefundService(_orderService);
    }

    [Fact]
    public async Task ProcessRefundAsync_WithValidCompletedOrder_ShouldApproveRefund()
    {
        // Arrange
        var order = new Order
        {
            Id = "ORD-001",
            Status = OrderStatus.Completed,
            TotalAmount = 500.00m
        };

        _orderService.GetOrderByIdAsync("ORD-001").Returns(order);

        // Act
        var result = await _sut.ProcessRefundAsync("ORD-001", 200.00m, "Changed my mind");

        // Assert
        result.Status.Should().Be(RefundStatus.Approved);
        result.Amount.Should().Be(200.00m);
        result.OrderId.Should().Be("ORD-001");
    }

    [Fact]
    public async Task ProcessRefundAsync_WithAmountExceedingTotal_ShouldRejectRefund()
    {
        // Arrange
        var order = new Order
        {
            Id = "ORD-001",
            Status = OrderStatus.Completed,
            TotalAmount = 500.00m
        };

        _orderService.GetOrderByIdAsync("ORD-001").Returns(order);

        // Act
        var result = await _sut.ProcessRefundAsync("ORD-001", 600.00m, "Full refund please");

        // Assert
        result.Status.Should().Be(RefundStatus.Rejected);
    }
}
