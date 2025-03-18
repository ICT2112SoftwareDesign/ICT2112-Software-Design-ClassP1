using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DataSource.Mapper;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Services.Forecast;
using CleanBrilliantCompany.Services.Notification;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<IStockPredictionService, SimpleStockPrediction>();
builder.Services.AddScoped<IScenarioPricingService, SimplePriceScenario>();
builder.Services.TryAddEnumerable(new[]
{
    ServiceDescriptor.Scoped<INotificationService, EmailNotification>(),
    ServiceDescriptor.Scoped<INotificationService, InAppNotification>()
});
builder.Services.AddScoped<IForecastRepository, ForecastMapper>();
builder.Services.AddScoped<IForecastingFacade, ForecastFacade>();

builder.Services.AddScoped<ForecastControl>();



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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
