using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Management;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Observers;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register the observer first
builder.Services.AddSingleton<ICustomerQueryObserver, CustomerSystemLogger>();

// Then the mapper (which depends on the observer)
builder.Services.AddSingleton<ICustomerDatabase>(provider => {
    var observer = provider.GetRequiredService<ICustomerQueryObserver>();
    return new CustomerMapper(connectionString, observer);
});

// 1. Register the Review Observer
builder.Services.AddSingleton<IReviewQueryObserver, ReviewActivityLogger>(); // You can change to another implementation later

// 2. Register the ReviewDatabase Mapper (depends on IReviewQueryObserver)
builder.Services.AddSingleton<IReviewDatabase>(provider =>
{
    var observer = provider.GetRequiredService<IReviewQueryObserver>();
    return new ReviewMapper(connectionString, observer);
});

// Register the Order Observer (OrderSystemLogger)
builder.Services.AddSingleton<IOrderQueryObserver, OrderSystemLogger>();

// Register the OrderMapper (depends on IOrderQueryObserver)
builder.Services.AddSingleton<IOrderDatabase>(provider =>
{
    var observer = provider.GetRequiredService<IOrderQueryObserver>();
    return new OrderMapper(connectionString, observer);
});

builder.Services.AddTransient<IOrder, OrderFulfilmentManagement>();

// Make sure OrderManagement is also registered (though it might not be needed for this flow)
builder.Services.AddTransient<OrderManagement>();

// Finally the management (which depends on the mapper)
builder.Services.AddTransient<CustomerManagement>();

builder.Services.AddTransient<SupportManagement>();
builder.Services.AddTransient<ChatbotService>();
builder.Services.AddScoped<IProduct, ProductManagement>(); 
builder.Services.AddScoped<IWishlistManagement, WishlistManagement>();
builder.Services.AddSingleton<IOrderDatabase>(provider => 
    new OrderMapper(connectionString, provider.GetRequiredService<IOrderQueryObserver>()));
builder.Services.AddTransient<IOrder, OrderFulfilmentManagement>(); // This is the key line
builder.Services.AddTransient<OrderFulfilmentManagement>();
builder.Services.AddTransient<OrderManagement>();
builder.Services.AddTransient<CartManagement>();
builder.Services.AddSingleton<ICartDatabase>(new CartMapper(connectionString));
builder.Services.AddSingleton<IWishlistDatabase>(new WishlistMapper(connectionString));
builder.Services.AddTransient<IShippingAgents, ShippingAgents>();
builder.Services.AddTransient<ICartManagement, CartManagement>();
builder.Services.AddTransient<WishlistManagement>();
builder.Services.AddTransient<ReviewManagement>();
builder.Services.AddScoped<OrderFulfilmentManagement>();
builder.Services.AddScoped<IRefundDatabase>(provider => new RefundMapper(connectionString));
builder.Services.AddScoped<IRefundQuery, RefundManagement>();
builder.Services.AddScoped<IRefundDetails, RefundDetails>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BeforeLoginPage}/{action=Login}/{id?}");


app.Run();
