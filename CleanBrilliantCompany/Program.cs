using CleanBrilliantCompany.Data;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IIngredientDB, IngredientGateway>();
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
    defaults: new { controller = "ToxicityPage", action = "Index" });


app.Run();
