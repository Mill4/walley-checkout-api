# Walley Backend Developer Code Challenge Solution

## Task 1
- Added new method to OrderController CreateOrder that takes in OrderCreationRequest containing customer name, email, and order lines
- Changed the signature in CreateOrderAsync in OrderService similar to other methods in the services which accepts only the input given
- CreateOrderAsync validates the input for empty name and email, empty order, and invalid amounts
- Added new status in Order.cs "Rejected" similar to RefundStatus to not accept invalid orders

## Task 2
- Added await for this call in RefundService: var order = await _orderService.GetOrderByIdAsync(orderId);
- Task is never null here and the order == null check evaluates to false always even when order was not found and then NullReferenceException is thrown later
- If you call order.Result when task is not awaited, the thread is blocked