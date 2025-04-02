using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Domain;
using CleanBrilliantCompany.DomainControl;
using CleanBrilliantCompany.Interfaces;  // Ensure this matches your actual namespace
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.CalculatorImplementation;
using CleanBrilliantCompany.Models.Control;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddScoped<ShipmentControl>();
builder.Services.AddScoped<ProductControl>();
builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<ItemControl>();
builder.Services.AddScoped<ItemMapper>();
builder.Services.AddScoped<GoalManagement>();

// Mappers
builder.Services.AddScoped<IProductCarbonFootprintDB, ProductCFMapper>();
builder.Services.AddScoped<IItemCarbonFootprintDB, ItemCFMapper>();
builder.Services.AddScoped<IOrderCarbonFootprintDB, OrderCFMapper>();
builder.Services.AddScoped<IProductDatabase, ProductMapper>();
builder.Services.AddScoped<IItemDatabase, ItemMapper>();
builder.Services.AddScoped<IRoutingService, RoutingAPI>();

// Product CF Controls
builder.Services.AddScoped<IProductCF, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProductCFManagement, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProductCFQuery, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProduct, ProductControl>();

// Item CF Controls
builder.Services.AddScoped<IItemCF, ItemCarbonFootprintControl>();
builder.Services.AddScoped<IItemCFManagement, ItemCarbonFootprintControl>();
builder.Services.AddScoped<IItemCFQuery, ItemCarbonFootprintControl>();
builder.Services.AddScoped<IItem, ItemControl>();

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

// Register async dependencies
builder.Services.AddScoped<IIngredientDB, IngredientGateway>();
builder.Services.AddScoped<IToxicityClassificationStrategy, ToxicityClassificationStrategy>();
builder.Services.AddScoped<IToxicity, IngredientToxicityAnalysisSDM>();
builder.Services.AddScoped<IGoalsDB, GoalsGateway>();
builder.Services.AddScoped<IGoals, GoalManagement>();

// Alert services
builder.Services.AddScoped<IAlertsDB, Alert_Gateway>();
builder.Services.AddScoped<IEmissionDataService, EmissionDataService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddHostedService<MonthlyGoalCheckService>();
builder.Services.AddSignalR();

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

// Map the hub endpoint before controller routes
app.MapHub<AlertHub>("/alertHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DashboardPage}/{action=Dashboard}/{id?}");

app.MapControllerRoute(
    name: "feedback",
    pattern: "feedback",
    defaults: new { controller = "Feedback", action = "Index" });

app.MapControllerRoute(
    name: "toxicity",
    pattern: "toxicity",
    defaults: new { Controller = "Toxicity", action = "Index" });

app.MapControllerRoute(
    name: "toxicityByProductName",
    pattern: "toxicity/product/{productName}",
    defaults: new { Controller = "Toxicity", action = "ViewByProductName" });

app.MapControllerRoute(
    name: "goalsManagement",
    pattern: "Goals",
    defaults: new { controller = "GoalsPage", action = "GoalsManagement" }
);

app.MapControllerRoute(
    name: "goalsCreation",
    pattern: "Goals/GoalsCreation",
    defaults: new { controller = "GoalsPage", action = "GoalsCreation" }
);

app.MapControllerRoute(
    name: "goalsModification",
    pattern: "Goals/GoalsModification",
    defaults: new { controller = "GoalsPage", action = "GoalsModification" }
);

app.MapControllerRoute(
    name: "alerts",
    pattern: "alerts",
    defaults: new { controller = "Alerts", action = "Index" }
);

app.Run();