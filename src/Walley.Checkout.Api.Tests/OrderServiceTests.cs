using FluentAssertions;
using Walley.Checkout.Api.Models;
using Walley.Checkout.Api.Services;
using Xunit;

namespace Walley.Checkout.Api.Tests;

public class OrderServiceTests
{
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _sut = new OrderService();
    }

    [Fact]
    public async Task GetAllOrdersAsync_ReturnsAllOrders()
    {
        // Act
        var result = await _sut.GetAllOrdersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3); // Based on seeded data in OrderService constructor
    }

    [Fact]
    public async Task GetOrderByIdAsync_WithExistingId_ReturnsOrder()
    {
        // Arrange
        var existingOrderId = "ORD-001";

        // Act
        var result = await _sut.GetOrderByIdAsync(existingOrderId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(existingOrderId);
    }

    [Fact]
    public async Task GetOrderByIdAsync_WithInvalidId_ThrowsInvalidOrderIdException()
    {
        // Arrange
        var invalidOrderId = "RANDOM-ORD";

        // Act
        Action action = () => _sut.GetOrderByIdAsync(invalidOrderId).Wait();

        // Assert
        action.Should().Throw<InvalidOrderIdException>()
            .WithMessage($"Invalid order ID format: {invalidOrderId}");
    }

    [Fact]
    public async Task GetOrderByIdAsync_WithNonExistingId_ThrowsOrderNotFoundException()
    {
        // Arrange
        var nonExistingOrderId = "ORD-999"; // Based on seeded data in OrderService constructor

        // Act
        Action action = () => _sut.GetOrderByIdAsync(nonExistingOrderId).Wait();

        // Assert
        action.Should().Throw<OrderNotFoundException>()
            .WithMessage($"Order with ID {nonExistingOrderId} not found");
    }

    [Fact]
    public async Task CreateOrderAsync_WithValidData_CreatesOrder()
    {
        // Arrange
        var customerName = "Alice Johnson";
        var customerEmail = "alice.johnson@example.com";
        var orderLines = new List<OrderLine>
        {
            new() { ProductName = "Mouse", Quantity = 1, UnitPrice = 10.00m },
            new() { ProductName = "Speaker", Quantity = 2, UnitPrice = 1399.00m }
        };

        // Act
        var result = await _sut.CreateOrderAsync(customerName, customerEmail, orderLines);

        // Assert
        result.Should().NotBeNull();
        result.CustomerName.Should().Be(customerName);
        result.CustomerEmail.Should().Be(customerEmail);
        result.TotalAmount.Should().Be(orderLines.Sum(l => l.LineTotal));
        result.Status.Should().Be(OrderStatus.Pending);
    }

    [Theory]
    [MemberData(nameof(GetInvalidOrderData))]
    public async Task CreateOrderAsync_WithInvalidData_ShouldRejectOrder(
        string customerName,
        string customerEmail,
        List<OrderLine> orderLines)
    {
        // Act
        var result = await _sut.CreateOrderAsync(customerName, customerEmail, orderLines);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(OrderStatus.Rejected);
    }

    public static IEnumerable<object[]> GetInvalidOrderData()
    {
        // Empty name
        yield return new object[] { "", "some@example.com", new List<OrderLine> 
        { 
            new() { ProductName = "Headset", Quantity = 1, UnitPrice = 1.0m } 
        } };

        // Empty email
        yield return new object[] { "Matti", "", new List<OrderLine>
        {
            new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 1.0m }
        } };

        // No orders
        yield return new object[] { "Bob", "bob@example.com", new List<OrderLine>() };

        // Invalid quantity
        yield return new object[] { "Carol", "carol@example.com", new List<OrderLine>
        {
            new() { ProductName = "Mouse", Quantity = 0, UnitPrice = 10.00m }
        } };

        // Invalid unit price
        yield return new object[] { "Dave", "dave@example.com", new List<OrderLine>
        {
            new() { ProductName = "Speaker", Quantity = 1, UnitPrice = -1399.00m }
        } };
    }
}
