using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Create the ShippingAgentDBService class in the Services folder first
builder.Services.AddScoped<ShippingAgentMapper>();
builder.Services.AddScoped<IShippingAgent, ShippingAgentMapper>();
// This line was trying to register an interface as its own implementation
// Change it to use a concrete implementation class
builder.Services.AddScoped<IShippingAgentDB>();

// Build the app first
var app = builder.Build();

// Use proper logging instead of BuildServiceProvider
app.Lifetime.ApplicationStarted.Register(() => {
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var mapper = app.Services.GetRequiredService<ShippingAgentMapper>();
    
    try {
        var agents = mapper.GetShippingAgentsAsync().Result.ToList();
        logger.LogInformation($"Debug: Found {agents.Count} shipping agents");
    }
    catch (Exception ex) {
        logger.LogError(ex, "Error fetching shipping agents during startup");
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();