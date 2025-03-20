using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services; // Ensure services is properly assigned
var config = builder.Configuration;


// Shipping Agent DB
var shippingAgentDB = new ShippingAgentMapper(config);
shippingAgentDB.FetchShippingAgents();

// Add services to the container.
builder.Services.AddControllersWithViews();

// In Program.cs or Startup.cs
services.AddScoped<ShippingAgentMapper>();
services.AddScoped<IShippingAgentService, ShippingAgentService>();

// Register DatabaseService
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
