using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Domain;
using CleanBrilliantCompany.Interfaces;  // Ensure this matches your actual namespace
using CleanBrilliantCompany.Models;

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

builder.Services.AddScoped<ICarbonRepositoryQuery, CarbonFootprintMapper>();
builder.Services.AddScoped<ICarbonRepositoryStatusQuery, CarbonFootprintRepositoryControl>();
builder.Services.AddScoped<ICarbonManagerQuery, CarbonFootprintManagerControl>();
builder.Services.AddScoped<ICarbonCalculatorQuery, CarbonFootprintCalculatorControl>();

builder.Services.AddScoped<CarbonFootprintManagerControl>();
builder.Services.AddScoped<CarbonFootprintCalculatorControl>();
builder.Services.AddScoped<CarbonFootprintRepositoryControl>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "feedback",
    pattern: "feedback",
    defaults: new { controller = "Feedback", action = "Index" });

app.Run();