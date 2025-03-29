using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Domain;
using CleanBrilliantCompany.DomainControl;
using CleanBrilliantCompany.Interfaces;  // Ensure this matches your actual namespace
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.CalculatorImplementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Database Configuration
builder.Services.AddScoped<FeedbackRepository>();

// Register Facades
builder.Services.AddScoped<StaffFeedbackFacade>();
builder.Services.AddScoped<ManageFeedbackFacade>();

// Register Interfaces and Implementations
builder.Services.AddScoped<IFeedbackSubmission, FeedbackSubmission>();
builder.Services.AddScoped<IFeedbackRetrieval, FeedbackRetrieval>();
builder.Services.AddScoped<IFeedbackManagement, FeedbackManagement>();

// Register your services
builder.Services.AddScoped<ProductCarbonFootprintControl>();
builder.Services.AddScoped<ItemCarbonFootprintControl>();
builder.Services.AddScoped<OrderCarbonFootprintControl>();
builder.Services.AddScoped<ProductControl>();
builder.Services.AddScoped<ProductMapper>();

// Mappers
builder.Services.AddScoped<IProductCarbonFootprintDB, ProductCFMapper>();
builder.Services.AddScoped<IItemCarbonFootprintDB, ItemCFMapper>();
builder.Services.AddScoped<IOrderCarbonFootprintDB, OrderCFMapper>();
builder.Services.AddScoped<IProductDatabase, ProductMapper>();


// Product CF Controls
builder.Services.AddScoped<IProductCF, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProductCFManagement, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProductCFQuery, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProduct, ProductControl>();

// Item CF Controls
builder.Services.AddScoped<IItemCF, ItemCarbonFootprintControl>();
builder.Services.AddScoped<IItemCFManagement, ItemCarbonFootprintControl>();
builder.Services.AddScoped<IItemCFQuery, ItemCarbonFootprintControl>();

// Order CF Controls
builder.Services.AddScoped<IOrderCF, OrderCarbonFootprintControl>();
builder.Services.AddScoped<IOrderCFManagement, OrderCarbonFootprintControl>();
builder.Services.AddScoped<IOrderCFQuery, OrderCarbonFootprintControl>();

// Calculator CF Controls
builder.Services.AddScoped<CarbonFootprintCalculatorControl>();
builder.Services.AddScoped<ICarbonData, CarbonFootprintCalculatorControl>();
builder.Services.AddScoped<IProductCFCalculator, CalculateProductCFImpl>();
builder.Services.AddScoped<IItemCFCalculator, CalculateItemCFImpl>();
builder.Services.AddScoped<IShipmentCFCalculator, CalculateShipmentCFImpl>();
builder.Services.AddScoped<IOrder, MockOrderService>();
builder.Services.AddScoped<IStorageDuration, StorageDurationStubImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Add this line before app.UseRouting();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DashboardPage}/{action=Dashboard}/{id?}");

app.MapControllerRoute(
    name: "feedback",
    pattern: "feedback",
    defaults: new { controller = "Feedback", action = "Index" });

app.Run();