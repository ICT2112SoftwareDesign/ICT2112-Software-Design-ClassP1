using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Dummy;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Mapper;
using DotNetEnv;
using CleanBrilliantCompany.Service;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
// var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");


// load environment variables from .env file 
Env.Load();
// get the connection string from the environment variables 
var connectionString = Env.GetString("CONNECTION_STRING");

// Configure services and add DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
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
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddScoped<IForecastDataAdapter, ForecastDataAdapter>();


builder.Services.AddScoped<ISales>(); //TODO to be modified with actual ISale
builder.Services.AddScoped<TempForecastIProduct>(); //TODO to be modified with actual ISale
builder.Services.AddSession();


builder.Services.AddMemoryCache();




// Add services to the container.
builder.Services.AddControllersWithViews();

// register aging mapper to use fakedb context 
//builder.Services.AddScoped<AgingMapper>(); 

builder.Services.AddScoped<AgingRepo, AgingMapper>();

// register the aging control 
builder.Services.AddScoped<AgingControl>();


// simulated version  (for product batches and stockhistory)
builder.Services.AddDbContext<SimulatedDbContext>(options =>
    options.UseSqlServer(connectionString));

//register the fakebatch interface 
builder.Services.AddScoped<FakeBatchInterface>();
//register the fakeproduct interface     
builder.Services.AddScoped<FakeProductInterface>();
builder.Services.AddScoped<IAlertService, InAppAlert>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CostMapper>();
builder.Services.AddScoped<CostControl>();
builder.Services.AddScoped<IProduct, MockProduct>(); // Simulation
builder.Services.AddScoped<InventoryControl>();
builder.Services.AddScoped<IInventoryRepository, InventoryMapper>();

builder.Services.AddScoped<ILogger<CostDashboardRdm>, Logger<CostDashboardRdm>>(); 
builder.Services.AddScoped<IItem, CostDataRetrievalService>();
builder.Services.AddScoped<IBatch, CostDataRetrievalService>();
builder.Services.AddScoped<IManufacturer, CostDataRetrievalService>();
builder.Services.AddScoped<ManufacturerRepo, ManufacturerMapper>();

// register the manufacturer control 
builder.Services.AddScoped<ManufacturerControl>(); 
builder.Services.AddScoped<FakeReorderInterface>(); 


var app = builder.Build();

//// Test the database connection
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    try
//    {
//        dbContext.Database.CanConnect();
//        Console.WriteLine("Database connection successful!");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Database connection failed: {ex.Message}");
//    }
//}

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
