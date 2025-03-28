using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Controllers;
using CleanBrilliantCompany.Mapper;
using CleanBrilliantCompany.Interfaces;

var builder = WebApplication.CreateBuilder(args);


//string connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

//// string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
////var configuration = builder.Configuration;
////builder.Services.AddSingleton<IConfiguration>(configuration);
//builder.Services.AddSingleton(connectionString);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnectionString' is missing or empty.");
}
builder.Services.AddSingleton(connectionString);


// using CleanBrilliantCompany.Interfaces;


// var builder = WebApplication.CreateBuilder(args);

// // string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// var configuration = builder.Configuration;
// builder.Services.AddSingleton<IConfiguration>(configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add this before `var app = builder.Build();`
builder.Services.AddScoped<IWarehouse, ItemControl>(); // Use the correct implementation
builder.Services.AddScoped<IItemQuery, ItemControl>();
builder.Services.AddScoped<IItemUpdate, ItemControl>();
builder.Services.AddScoped<IItem, ItemControl>();
builder.Services.AddScoped<IReserve, ItemControl>();
builder.Services.AddScoped<IOrderFufilment, ItemControl>();
builder.Services.AddScoped<IRefundDetails, ItemControl>();
builder.Services.AddScoped<IItemCreation, ItemControl>();
builder.Services.AddScoped<IReturnForm, ItemControl>();

builder.Services.AddScoped<iProduct, ProductControl>();
builder.Services.AddScoped<iProductQuantity, ProductControl>();
builder.Services.AddScoped<iReorderRequest, ReorderRequestManagement>();
builder.Services.AddScoped<iManufacturer, ProductControl>();

builder.Services.AddScoped<iReturnFormDatabase<ReturnForm>, ReturnFormMapper>();
builder.Services.AddScoped<ReturnFormControl>();
builder.Services.AddScoped<ReturnFormMapper>();
builder.Services.AddScoped<ReturnFormController>();
builder.Services.AddScoped<ItemControl>();
builder.Services.AddScoped<ProductControl>();
builder.Services.AddScoped<iProduct, ProductControl>();

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
