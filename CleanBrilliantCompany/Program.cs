using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DataSource.Mapper;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using CleanBrilliantCompany.Services.Forecast;
using CleanBrilliantCompany.Services.Notification;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);// Load .env file
Env.Load();

// Get the connection string from environment variables
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("SQL_CONNECTION_STRING is not set. Please check your .env file.");
}

// Add DbContext (if using Entity Framework Core)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// Register services

//builder.Services.TryAddEnumerable(new[]
//{
//    ServiceDescriptor.Scoped<INotificationService, EmailNotification>(),
//    ServiceDescriptor.Scoped<INotificationService, InAppNotification>()
//});
builder.Services.TryAddEnumerable(new[]
{
    ServiceDescriptor.Scoped<IPredictionService, StockForecastPrediction>(),
    ServiceDescriptor.Scoped<IPredictionService, PriceScenarioPrediction>()
});

builder.Services.AddScoped<IForecastRepository, ForecastMapper>();
//builder.Services.AddScoped<IForecastingFacade, ForecastFacade>();
//builder.Services.AddScoped<ForecastControl>();
builder.Services.AddScoped<MetricFactory>();
builder.Services.AddScoped<ForecastFacade>();
builder.Services.AddScoped<IAlert, TopDemandAlert>();




builder.Services.AddScoped<ISales>(); //TODO to be modified with actual ISale
builder.Services.AddScoped<IProduct>(); //TODO to be modified with actual ISale
builder.Services.AddSession();
builder.Services.AddMemoryCache();




// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(name: "default", pattern: "{controller=Forecast}/{action=fetchDashboardData}/{id?}")
    .WithStaticAssets();

app.Run();
