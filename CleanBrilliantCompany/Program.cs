using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mapper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register services properly - using the new approach
builder.Services.AddScoped<ShippingAgentMapper>();
builder.Services.AddScoped<_IShippingAgentDB, ShippingAgentMapper>();

// Remove DbContext registration and use direct SQL connection
// Create ShippingAgentMapper instance after services are registered to demonstrate data fetching
// Note: This is for debugging only and should be removed in production
var serviceProvider = builder.Services.BuildServiceProvider();
var shippingAgentMapper = serviceProvider.GetRequiredService<ShippingAgentMapper>();
var agents = shippingAgentMapper.FetchShippingAgents();
Console.WriteLine($"Debug: Found {agents.Count} shipping agents");

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