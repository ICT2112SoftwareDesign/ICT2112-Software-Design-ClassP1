using Microsoft.EntityFrameworkCore;


DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSingleton<CostSimulation>();  

// ✅ Register CostMapper as a service
builder.Services.AddScoped<CostMapper>();

// ✅ Register CostControl (also requires logger)
builder.Services.AddScoped<CostControl>();

builder.Services.AddScoped<ILogger<CostDashboardRdm>, Logger<CostDashboardRdm>>();  // ✅ Added Logger
// ✅ Register Visualization Service
builder.Services.AddScoped<IVisualizationService, VisualizationService>();

builder.Services.AddScoped<IAlertService, InAppAlert>();

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

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CostPage}/{action=Index}/{id?}");

app.Run();
