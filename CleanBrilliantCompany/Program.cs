using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Models;
using QuestPDF.Infrastructure;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Mapper;

// -------------------------------
// Load Environment Variables
// -------------------------------
Env.Load();
var connectionString = Env.GetString("CONNECTION_STRING");
var apiKey = Env.GetString("OPENAI_API_KEY");

// -------------------------------
// Set QuestPDF Community License
// -------------------------------
QuestPDF.Settings.License = LicenseType.Community;

// -------------------------------
// .NET Runtime Configuration
// -------------------------------
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
// Optional: Force TLS 1.2+ to ensure compatibility with OpenAI
System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Database Configuration
// -------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// -------------------------------
// MVC Setup
// -------------------------------
builder.Services.AddControllersWithViews();

// -------------------------------
// Dev + Fake Data
// -------------------------------
builder.Services.AddSingleton<FakeDbContext>();
builder.Services.AddScoped<AgingRepo, AgingMapper>();

// -------------------------------
// OpenAI + Report Generation
// -------------------------------
builder.Services.AddHttpClient<IAIService, AIService>();
builder.Services.AddScoped<ReportGenerator>();
builder.Services.AddScoped<ReportControl>();
builder.Services.AddScoped<ReportRepo, ReportMapper>();
Console.WriteLine($"[Debug] OpenAI Key Length: {apiKey?.Length}");


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
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
