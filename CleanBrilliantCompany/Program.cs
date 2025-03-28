using Microsoft.EntityFrameworkCore;
using DotNetEnv; 

var builder = WebApplication.CreateBuilder(args);


// load environment variables from .env file 
Env.Load(); 
// get the connection string from the environment variables 
var connectionString = Env.GetString("CONNECTION_STRING"); 

// Console.WriteLine($"Connection String: {connectionString}");


// Configure services and add DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); 


// Add services to the container.
builder.Services.AddControllersWithViews();

// register aging mapper to use fakedb context 
//builder.Services.AddScoped<AgingMapper>(); 

builder.Services.AddScoped<ManufacturerRepo, ManufacturerMapper>();

// register the aging control 
builder.Services.AddScoped<ManufacturerControl>(); 


// // simulated version  (for product batches and stockhistory)
// builder.Services.AddDbContext<SimulatedDbContext>(options =>
//     options.UseSqlServer(connectionString)); 

// //register the fakebatch interface 
// builder.Services.AddScoped<FakeBatchInterface>(); 
// //register the fakeproduct interface     
// builder.Services.AddScoped<FakeProductInterface>(); 


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
    pattern: "{controller=ManufacturerPage}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();