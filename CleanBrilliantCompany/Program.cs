using CleanBrilliantCompany.Data;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register async dependencies
builder.Services.AddScoped<IIngredientDB, IngredientGateway>();
builder.Services.AddScoped<IToxicityClassificationStrategy, StandardToxicityClassificationStrategy>();
builder.Services.AddScoped<IToxicity, IngredientToxicityAnalysisSDM>();

builder.Services.AddScoped<IAlertsDB, Alert_Gateway>();
builder.Services.AddScoped<ICarbonNotification, CarbonNotification>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "toxicity",
    pattern: "toxicity",
    defaults: new { Controller = "Toxicity", action = "Index" });

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
    defaults: new { controller = "Alert", action = "Index" });

app.Run();
