using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models;
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
builder.Services.AddSingleton<ICustomerDatabase>(new CustomerMapper(connectionString));
builder.Services.AddTransient<CustomerManagement>();
builder.Services.AddTransient<SupportManagement>();
builder.Services.AddTransient<ChatbotService>();
builder.Services.AddScoped<IProduct, ProductManagement>(); 
builder.Services.AddTransient<OrderManagement>();
builder.Services.AddSingleton<IOrderDatabase>(new OrderMapper(connectionString));
builder.Services.AddTransient<CartManagement>();
builder.Services.AddSingleton<ICartDatabase>(new CartMapper(connectionString));
builder.Services.AddTransient<IShippingAgents, ShippingAgents>();



builder.Services.AddTransient<WishlistManagement>();
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
