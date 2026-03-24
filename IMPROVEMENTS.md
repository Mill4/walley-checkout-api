# Walley Backend Developer Code Improvements

- Upgrade to the latest and greatest .NET 10 for long term support
- Add CI/CD
- Move the request objects outside of the controller
- Currently it does not make sense in OrderService and RefundService to do separate checks for rejections, one if statement would do the same thing. BUT, if we wanted to provide some meaningful error message/error code, then this structure makes sense
- It's not immediately obvious how the order status changes from Pending - Confirmed - Completed