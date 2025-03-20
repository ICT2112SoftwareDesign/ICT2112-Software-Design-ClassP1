using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddScoped<IRefundDatabase>(provider => new RefundMapper(connectionString));
builder.Services.AddScoped<IRefundQuery, RefundManagement>();
builder.Services.AddScoped<IRefundDetails, RefundDetails>();
builder.Services.AddScoped<IOrder, MockOrderService>(); // change later when team5 is done

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
