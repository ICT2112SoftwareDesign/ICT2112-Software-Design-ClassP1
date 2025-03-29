using CleanBrilliantCompany.Data;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Controllers;

var builder = WebApplication.CreateBuilder(args);
// Enable logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Ensures logs appear in the console

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register async dependencies
builder.Services.AddScoped<IIngredientDB, IngredientGateway>();
builder.Services.AddScoped<IToxicityClassificationStrategy, ToxicityClassificationStrategy>();
builder.Services.AddScoped<IToxicity, IngredientToxicityAnalysisSDM>();

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
    name: "toxicityByProductName",
    pattern: "toxicity/product/{productName}",
    defaults: new { Controller = "Toxicity", action = "ViewByProductName" });

app.Run();
