using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Observers;
using Microsoft.Extensions.DependencyInjection;

using CleanBrilliantCompany.Data.SupportTicket;
using CleanBrilliantCompany.Models.SupportTicket;
using CleanBrilliantCompany.Interfaces.SupportTicket;

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

// Register the Cart Observer (e.g., CartSystemLogger)
builder.Services.AddSingleton<ICartQueryObserver, CartSystemLogger>();

// Register the CartMapper (depends on ICartQueryObserver)
builder.Services.AddSingleton<ICartDatabase>(provider =>
{
    var observer = provider.GetRequiredService<ICartQueryObserver>();
    return new CartMapper(connectionString, observer);
});

// Register the Order Observer (OrderSystemLogger)
builder.Services.AddSingleton<IOrderQueryObserver, OrderSystemLogger>();

// Register the OrderMapper (depends on IOrderQueryObserver)
builder.Services.AddSingleton<IOrderDatabase>(provider =>
{
    var observer = provider.GetRequiredService<IOrderQueryObserver>();
    return new OrderMapper(connectionString, observer);
});

// Finally the management (which depends on the mapper)
builder.Services.AddTransient<CustomerManagement>();
builder.Services.AddTransient<SupportManagement>();
builder.Services.AddTransient<IChatbot, ChatbotService>();
builder.Services.AddTransient<ISupportTicket, SupportTicketService>();
builder.Services.AddScoped<IProduct, ProductManagement>(); 
builder.Services.AddScoped<IWishlistManagement, WishlistManagement>();
builder.Services.AddScoped<IOrder, OrderManagement>();
builder.Services.AddTransient<OrderManagement>();
builder.Services.AddTransient<CartManagement>();
builder.Services.AddTransient<ICartManagement, CartManagement>();
builder.Services.AddSingleton<IWishlistDatabase>(new WishlistMapper(connectionString));
builder.Services.AddTransient<IShippingAgents, ShippingAgents>();
builder.Services.AddTransient<WishlistManagement>();
builder.Services.AddTransient<ReviewManagement>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adding services for Support Ticket
builder.Services.AddScoped<ISupportTicket, SupportTicketManagement>();
builder.Services.AddScoped<iSupportTicketQuery, SupportTicketManagement>();
builder.Services.AddScoped<SupportTicketManagement>();
builder.Services.AddScoped<SupportTicketTableDataGateway>(provider =>
    new SupportTicketTableDataGateway(connectionString!));
    
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
