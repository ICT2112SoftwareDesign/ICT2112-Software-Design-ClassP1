using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Controllers;
using CleanBrilliantCompany.Mapper;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ReturnFormControl>();
builder.Services.AddScoped<ReturnFormMapper>();
builder.Services.AddScoped<ReturnFormController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(

	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
