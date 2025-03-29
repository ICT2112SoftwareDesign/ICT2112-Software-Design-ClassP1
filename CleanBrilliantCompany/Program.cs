using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Dummy;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Mapper;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// load environment variables from .env file 
Env.Load();
// get the connection string from the environment variables 
var connectionString = Env.GetString("CONNECTION_STRING");

// Configure services and add DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();

// register aging mapper to use fakedb context 
//builder.Services.AddScoped<AgingMapper>(); 

builder.Services.AddScoped<AgingRepo, AgingMapper>();

// register the aging control 
builder.Services.AddScoped<AgingControl>();


// simulated version  (for product batches and stockhistory)
builder.Services.AddDbContext<SimulatedDbContext>(options =>
    options.UseSqlServer(connectionString));

//register the fakebatch interface 
builder.Services.AddScoped<FakeBatchInterface>();
//register the fakeproduct interface     
builder.Services.AddScoped<FakeProductInterface>();


// Configure Entity Framework with the connection string from appsettings.json
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProduct, MockProduct>(); // Simulation
builder.Services.AddScoped<InventoryControl>();
builder.Services.AddScoped<IInventoryRepository, InventoryMapper>();

var app = builder.Build();

//// Test the database connection
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    try
//    {
//        dbContext.Database.CanConnect();
//        Console.WriteLine("Database connection successful!");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Database connection failed: {ex.Message}");
//    }
//}

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
