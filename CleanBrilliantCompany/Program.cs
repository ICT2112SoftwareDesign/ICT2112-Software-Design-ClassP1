using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Mapper;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework with the connection string from appsettings.json
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DbContext with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InventoryDb"));

builder.Services.AddScoped<InventoryControl>();
builder.Services.AddScoped<IInventoryRepository, InventoryMapper>();

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
    pattern: "{controller=InventoryPage}/{action=ViewDashboard}/{id?}")
    .WithStaticAssets();

app.Run();
