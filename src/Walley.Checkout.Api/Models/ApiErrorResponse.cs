namespace Walley.Checkout.Api.Models;

public class ApiErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public int StatusCode { get; set; }
}

public class OrderNotFoundException(string message) : Exception(message);

public class InvalidOrderIdException(string message) : Exception(message);
