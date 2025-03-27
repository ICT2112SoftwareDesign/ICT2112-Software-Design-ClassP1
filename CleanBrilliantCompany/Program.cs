using CleanBrilliantCompany.Data.SupportTicket;
using CleanBrilliantCompany.Models.SupportTicket;
using CleanBrilliantCompany.Interfaces.SupportTicket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adding services for Support Ticket
builder.Services.AddScoped<ISupportTicket, SupportTicketManagement>();
builder.Services.AddScoped<iSupportTicketQuery, SupportTicketManagement>();
builder.Services.AddScoped<SupportTicketManagement>();
builder.Services.AddScoped<SupportTicketTableDataGateway>(provider =>
    new SupportTicketTableDataGateway(connectionString!));
    
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
