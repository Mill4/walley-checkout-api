using Walley.Checkout.Api.Middleware;
using Walley.Checkout.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Walley Checkout API",
        Version = "v1",
        Description = "A simplified checkout service for managing orders and refunds."
    });
});

builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddScoped<IRefundService, RefundService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
