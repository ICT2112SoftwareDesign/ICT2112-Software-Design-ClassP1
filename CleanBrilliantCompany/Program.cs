using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interfaces;
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

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register Observers
builder.Services.AddSingleton<ICustomerQueryObserver, CustomerSystemLogger>();
builder.Services.AddSingleton<IReviewQueryObserver, ReviewActivityLogger>();
builder.Services.AddSingleton<IOrderQueryObserver, OrderSystemLogger>();

// Register Database Mappers
builder.Services.AddSingleton<ICustomerDatabase>(provider => 
    new CustomerMapper(connectionString, provider.GetRequiredService<ICustomerQueryObserver>()));
builder.Services.AddSingleton<IReviewDatabase>(provider => 
    new ReviewMapper(connectionString, provider.GetRequiredService<IReviewQueryObserver>()));
builder.Services.AddSingleton<IOrderDatabase>(provider => 
    new OrderMapper(connectionString, provider.GetRequiredService<IOrderQueryObserver>()));
builder.Services.AddSingleton<ICartDatabase>(new CartMapper(connectionString));
builder.Services.AddSingleton<IWishlistDatabase>(new WishlistMapper(connectionString));
builder.Services.AddSingleton<IRefundDatabase>(new RefundMapper(connectionString));

// Register Management Services
builder.Services.AddTransient<CustomerManagement>();
builder.Services.AddTransient<SupportManagement>();
builder.Services.AddTransient<ChatbotService>();
builder.Services.AddScoped<IProduct, ProductManagement>(); 
builder.Services.AddScoped<IWishlistManagement, WishlistManagement>();
builder.Services.AddTransient<OrderManagement>();
builder.Services.AddTransient<CartManagement>();
builder.Services.AddTransient<IShippingAgents, ShippingAgents>();
builder.Services.AddTransient<ICartManagement, CartManagement>();
builder.Services.AddTransient<WishlistManagement>();
builder.Services.AddTransient<ReviewManagement>();
builder.Services.AddScoped<OrderFulfilmentManagement>();
builder.Services.AddScoped<IRefundQuery, RefundManagement>();
builder.Services.AddScoped<IRefundDetails, RefundDetails>();

// Register Dashboard Services
builder.Services.AddScoped<DashboardManagement>();
builder.Services.AddScoped<IOrder>(provider => 
    provider.GetRequiredService<OrderManagement>());
builder.Services.AddScoped<IRefundQuery>(provider => 
    provider.GetRequiredService<RefundManagement>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BeforeLoginPage}/{action=Login}/{id?}");

app.Run();