# Walley Backend Developer Code Challenge

## Introduction

Welcome! We're excited to see how you approach building backend payment services at Walley.

This challenge uses a simplified **Checkout API** — a service that handles orders and refunds. The project is partially built: one endpoint works, the structure is in place, and your job is to extend, fix, and test it.

**Time expectation:** 2-3 hours

**What we're looking for:**
- Solid C# and .NET fundamentals
- Clean, readable code that follows existing patterns
- Thoughtful error handling and validation
- Testing instincts
- Pragmatic decision-making

## Getting Started

```bash
cd src/Walley.Checkout.Api
dotnet restore
dotnet run
```

The API runs at `https://localhost:5001` (or `http://localhost:5000`). Swagger UI is available at `/swagger`.

To run the existing tests:

```bash
cd src/Walley.Checkout.Api.Tests
dotnet test
```

## The Scenario

You're working on Walley's Checkout API. The service handles customer orders — creating them, retrieving them, processing refunds, and calculating totals. A colleague started building it but moved to another team. You're picking up where they left off.

Take a few minutes to read through the existing code before starting. The project has a clear structure: controllers, services, models — and one fully working endpoint (`GET /api/orders`) as a reference.

## Your Tasks

### Task 1: Add the "Create Order" Endpoint

Implement `POST /api/orders` that accepts a new order and returns the created order.

**Requirements:**
- Accept a request body with customer info and order lines
- Validate the input (no empty orders, valid amounts, required fields)
- Calculate the total from the order lines
- Assign a new ID and set the initial status to `Pending`
- Return `201 Created` with the order and a `Location` header

Look at the existing models and the `GET` endpoint for conventions to follow.

### Task 2: Fix the Bug in `RefundService`

The `ProcessRefundAsync` method in `RefundService` has a bug. It doesn't work as intended.

Find it, fix it, and briefly explain (in a code comment) what was wrong.

### Task 3: Write Unit Tests for `OrderService`

The `OrderService` has no tests. Write unit tests that cover the important behaviors.

xUnit and NSubstitute are already referenced in the test project. Focus on meaningful tests — quality over quantity.

### Task 4: Improve Error Handling

The `GET /api/orders/{id}` endpoint currently returns a generic `500` if anything goes wrong. Improve the error handling so the API returns appropriate status codes and useful error responses.

Consider: what should happen when an order isn't found? When the ID format is invalid? How should unexpected errors be handled?

### Task 5 (Open-ended): What Would You Improve?

If you had more time, what would you change or add? Write your thoughts in a short section at the bottom of this README or as a separate `IMPROVEMENTS.md` file.

This isn't about writing a lot — a few bullet points or a paragraph is fine. We're curious about what catches your eye.

## Project Structure

```
src/
├── Walley.Checkout.Api/
│   ├── Controllers/
│   │   └── OrdersController.cs      # API endpoints
│   ├── Services/
│   │   ├── IOrderService.cs          # Order service interface
│   │   ├── OrderService.cs           # Order business logic
│   │   ├── IRefundService.cs         # Refund service interface
│   │   └── RefundService.cs          # Refund logic (has a bug!)
│   ├── Models/
│   │   ├── Order.cs                  # Order domain model
│   │   ├── OrderLine.cs              # Order line item
│   │   ├── Refund.cs                 # Refund model
│   │   └── ApiErrorResponse.cs       # Error response DTO
│   ├── Middleware/
│   │   └── ExceptionMiddleware.cs    # Global exception handler
│   ├── Program.cs
│   └── Walley.Checkout.Api.csproj
├── Walley.Checkout.Api.Tests/
│   ├── RefundServiceTests.cs         # Example tests (for reference)
│   └── Walley.Checkout.Api.Tests.csproj
```

## Tips

- **Follow existing patterns** — consistency matters more than cleverness
- **Start with Task 1**, it builds your understanding of the codebase
- **Don't over-engineer** — this is a small service, treat it like one
- **Commit as you go** — we like seeing your thought process
- **Read the existing code first** — the answers are in the patterns

## Questions?

If anything is unclear, reach out to me at bob.jelica@walley.se — we're happy to help.

---

**Note:** We don't expect perfection in 2-3 hours. We want to see how you think, prioritize, and write code in a real-world codebase. Quality over quantity.

Good luck! 🚀
