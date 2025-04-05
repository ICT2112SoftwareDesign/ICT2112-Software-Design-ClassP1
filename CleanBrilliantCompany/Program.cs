using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Controllers;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DataSource.Mapper;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Mapper;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Factory;
using CleanBrilliantCompany.Models.Forecast;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Services.Forecast;
using CleanBrilliantCompany.Services.Notification;
using CleanBrilliantCompany.Service;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Set the license for QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

// Load environment variables from .env file
DotNetEnv.Env.Load();

// Try to get connection string from environment variable first
string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// Fallback to appsettings.json if env var not found
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

// Final check to ensure a valid connection string was found
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string not found in environment variables or appsettings.json.");
}

builder.Services.AddSingleton(connectionString);

// using CleanBrilliantCompany.Interfaces;


// var builder = WebApplication.CreateBuilder(args);

// // string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// var configuration = builder.Configuration;
// builder.Services.AddSingleton<IConfiguration>(configuration);

// Add services to the container.



// get apikey from env
var apiKey = Env.GetString("OPENAI_API_KEY");

// Configure services and add DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Add DbContext (if using Entity Framework Core)
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString)
// );

// Register services

//builder.Services.TryAddEnumerable(new[]
//{
//    ServiceDescriptor.Scoped<INotificationService, EmailNotification>(),
//    ServiceDescriptor.Scoped<INotificationService, InAppNotification>()
//});
builder.Services.TryAddEnumerable(
    new[]
    {
        ServiceDescriptor.Scoped<IPredictionService, StockForecastPrediction>(),
        ServiceDescriptor.Scoped<IPredictionService, PriceScenarioPrediction>(),
    }
);

builder.Services.AddScoped<IForecastRepository, ForecastMapper>();

//builder.Services.AddScoped<IForecastingFacade, ForecastFacade>();
//builder.Services.AddScoped<ForecastControl>();
builder.Services.AddScoped<MetricFactory>();
builder.Services.AddScoped<ForecastFacade>();
builder.Services.AddScoped<IAlert, TopDemandAlert>();
builder.Services.AddScoped<IForecastDataAdapter, ForecastDataAdapter>();
builder.Services.AddScoped<IDashboardFacade, DashboardFacade>();

builder.Services.AddScoped<IOrderRange>(); //TODO to be modified with actual ISale
builder.Services.AddScoped<TempForecastIProduct>(); //TODO to be modified with actual ISale
//builder.Services.AddSession();

builder.Services.AddMemoryCache();

// -------------------------------
// MVC Setup
// -------------------------------
builder.Services.AddControllersWithViews();

// Add this before `var app = builder.Build();`
builder.Services.AddScoped<IWarehouse, ItemControl>(); // Use the correct implementation
builder.Services.AddScoped<IItemQuery, ItemControl>();
builder.Services.AddScoped<IItemUpdate, ItemControl>();
builder.Services.AddScoped<IItem, ItemControl>();
builder.Services.AddScoped<IReserve, ItemControl>();
builder.Services.AddScoped<IOrderFufilment, ItemControl>();
builder.Services.AddScoped<IRefundDetails, ItemControl>();
builder.Services.AddScoped<IItemCreation, ItemControl>();
builder.Services.AddScoped<IReturnForm, ItemControl>();
builder.Services.AddScoped<IItemDetails, ItemControl>();


builder.Services.AddScoped<IProduct, ProductControl>();
builder.Services.AddScoped<IProductQuantity, ProductControl>();
builder.Services.AddScoped<IBatch, ProductControl>();
builder.Services.AddScoped<IManufacturer, ProductControl>();
builder.Services.AddScoped<iReorderRequest, ReorderRequestManagement>();
builder.Services.AddScoped<iItem, CostDataRetrievalService>();
builder.Services.AddScoped<iBatch, CostDataRetrievalService>();
builder.Services.AddScoped<iManufacturer, CostDataRetrievalService>();

// Lazy resolver for breaking circular dependency
builder.Services.AddScoped(provider => new Lazy<IItemCreation>(
    () => provider.GetRequiredService<IItemCreation>()
));

builder.Services.AddScoped<iReturnFormDatabase<ReturnForm>, ReturnFormMapper>();
builder.Services.AddScoped<ReturnFormControl>();
builder.Services.AddScoped<ReturnFormMapper>();
builder.Services.AddScoped<ReturnFormController>();
builder.Services.AddScoped<TransferControl>();
builder.Services.AddScoped<TransferMapper>();
builder.Services.AddScoped<TransferController>();
builder.Services.AddScoped<ItemControl>();
builder.Services.AddScoped<ProductControl>();
// builder.Services.AddScoped<ProductFactory>();
builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<ProductFactory, LiquidProductFactory>();
builder.Services.AddScoped<ProductFactory, SolidProductFactory>();


//==================================================
//aging
builder.Services.AddScoped<IAgingRepository, AgingMapper>();
builder.Services.AddScoped<AgingControl>();
//================================================


builder.Services.AddScoped<IAlertService, InAppAlert>();
// builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CostMapper>();
builder.Services.AddScoped<CostControl>();
//builder.Services.AddScoped<Team6IProduct, MockProduct>(); // Simulation
builder.Services.AddScoped<InventoryControl>();
builder.Services.AddScoped<IInventoryRepository, InventoryMapper>();

builder.Services.AddScoped<ILogger<CostDashboardRdm>, Logger<CostDashboardRdm>>();
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
builder.Services.AddScoped<IReportRepository, ReportGateway>();
//builder.Services.AddScoped<AgingControl>();
//builder.Services.AddScoped<ManufacturerControl>();
//builder.Services.AddScoped<CostControl>();
builder.Services.AddScoped<IForecastReportDetails, ForecastFacade>();

builder.Services.AddScoped<IAnalyticsReportDetails>();
builder.Services.AddScoped<IAnalyticsReportDetails, DashboardFacade>();
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
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Dashboards}/{id?}");

app.Run();
