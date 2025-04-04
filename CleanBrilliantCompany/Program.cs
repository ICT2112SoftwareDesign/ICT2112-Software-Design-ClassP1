using CleanBrilliantCompany.Data;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Controllers;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Hubs;
using CleanBrilliantCompany.DomainControl;


var builder = WebApplication.CreateBuilder(args);
// Enable logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Ensures logs appear in the console

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IItemCarbonFootprintDB, ItemCFMapper>();
builder.Services.AddScoped<IOrderCarbonFootprintDB, OrderCFMapper>();
builder.Services.AddScoped<IProductCarbonFootprintDB, ProductCFMapper>();

builder.Services.AddScoped<IProductCF, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProductCFManagement, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProductCFQuery, ProductCarbonFootprintControl>();
builder.Services.AddScoped<IProduct, ProductControl>();
builder.Services.AddScoped<IProductQuery, ProductControl>();;
builder.Services.AddScoped<IProductDatabase, ProductMapper>();
builder.Services.AddScoped<ProductMapper>();


// Register async dependencies
builder.Services.AddScoped<IIngredientDB, IngredientGateway>();
builder.Services.AddScoped<IToxicityClassificationStrategy, ToxicityClassificationStrategy>();
builder.Services.AddScoped<IToxicity, IngredientToxicityAnalysisSDM>();

// Alert services
builder.Services.AddScoped<IAlertsDB, Alert_Gateway>();
builder.Services.AddScoped<ICarbonNotification, CarbonOrderItemAnalyticManager>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddHostedService<MonthlyGoalCheckService>();
builder.Services.AddSignalR();


builder.Services.AddScoped<IGoalsDB, GoalsGateway>();
builder.Services.AddScoped<GoalManagement>();
builder.Services.AddScoped<IGoals, GoalManagement>();
builder.Services.AddScoped<IPredictionStrategy, PredictionSSA>();
builder.Services.AddScoped<IPredictionStrategy, PredictionSMA>();

builder.Services.AddScoped<CarbonOrderItemAnalyticManager>();
builder.Services.AddScoped<IItemCF, ItemCarbonFootprintControl>();
builder.Services.AddScoped<IOrderCF, OrderCarbonFootprintControl>();
builder.Services.AddScoped<IProductCF, ProductCarbonFootprintControl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map the hub endpoint before controller routes
app.MapHub<AlertHub>("/alertHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
    name: "analyticsGraph",
    pattern: "analytics/graph",
    defaults: new { controller = "Analytics", action = "DisplayGraph" });

app.MapControllerRoute(
    name: "alerts",
    pattern: "alerts",
    defaults: new { controller = "Alert", action = "Index" }
);

// Sustainable Resource Inventory is Low
// URL: http://localhost:5258/sustainable-ingredient/resources
app.MapControllerRoute(
    name: "sustainableIngredient",
    pattern: "sustainable-ingredient/resources",
    defaults: new
    {
        controller = "SustainableIngredients",
        action = "Index"
    }
);

app.Run();
