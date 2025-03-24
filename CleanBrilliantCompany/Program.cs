using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Models;
using QuestPDF.Infrastructure;

// quest pdf community license
QuestPDF.Settings.License = LicenseType.Community;


var builder = WebApplication.CreateBuilder(args);


// load environment variables from .env file 
Env.Load();

// get the connection string from the environment variables 
var connectionString = Env.GetString("CONNECTION_STRING");

// Configure services and add DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();



// register fake context as a singleton 
builder.Services.AddSingleton<FakeDbContext>();


// register aging mapper to use fakedb context 
//builder.Services.AddScoped<AgingMapper>(); 

builder.Services.AddScoped<AgingRepo, AgingMapper>();


// connect to openai unsecured
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// report generation
builder.Services.AddHttpClient<IAIService, AIService>();
builder.Services.AddScoped<ReportGenerator>();
builder.Services.AddScoped<ReportControl>();




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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
