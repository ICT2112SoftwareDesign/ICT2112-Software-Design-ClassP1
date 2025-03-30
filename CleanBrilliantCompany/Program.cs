using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Dummy;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Mapper;
using DotNetEnv;
using CleanBrilliantCompany.Service;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DataSource.Mapper;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using CleanBrilliantCompany.Services.Forecast;
using CleanBrilliantCompany.Services.Notification;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuestPDF.Infrastructure;

using CleanBrilliantCompany.Services;
QuestPDF.Settings.License = LicenseType.Community;
DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
// var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");


// load environment variables from .env file 
Env.Load();
// get the connection string from the environment variables 
var connectionString = Env.GetString("CONNECTION_STRING");

// get apikey from env
var apiKey = Env.GetString("OPENAI_API_KEY");

// Configure services and add DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


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
builder.Services.AddScoped<IDashboardFacade, DashboardFacade>();

builder.Services.AddScoped<ISales>(); //TODO to be modified with actual ISale
builder.Services.AddScoped<TempForecastIProduct>(); //TODO to be modified with actual ISale
builder.Services.AddSession();


builder.Services.AddMemoryCache();




// -------------------------------
// MVC Setup
// -------------------------------
builder.Services.AddControllersWithViews();



// register fake context as a singleton 
builder.Services.AddSingleton<FakeDbContext>();


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

// -------------------------------
// OpenAI + Report Generation
// -------------------------------
builder.Services.AddHttpClient<IAIService, AIService>();
builder.Services.AddScoped<ReportGenerator>();
builder.Services.AddScoped<ReportControl>();
builder.Services.AddScoped<ReportRepo, ReportMapper>();
builder.Services.AddScoped<AgingControl>();
builder.Services.AddScoped<ManufacturerControl>();
builder.Services.AddScoped<CostControl>();

builder.Services.AddScoped<DashboardFacade>();
builder.Services.AddScoped<IDashboardFacade, DashboardFacade>();
Console.WriteLine($"[Debug] OpenAI Key Length: {apiKey?.Length}");
builder.Services.AddSession();

// -------------------------------
// Build & Run the App
// -------------------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Dashboards}/{id?}")
    .WithStaticAssets();

app.Run();
