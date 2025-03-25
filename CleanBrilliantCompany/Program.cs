using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Refund;
using CleanBrilliantCompany.Interfaces.StaffAuth;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.StaffAuth;
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

// Adding services for Staff
builder.Services.AddScoped<StaffManagement>();
builder.Services.AddScoped<StaffAuthentication>();
builder.Services.AddScoped<IStaffDatabase>(sp => new StaffMapper(connectionString));

// Adding services for Refund
builder.Services.AddScoped<IRefundDatabase>(provider => new RefundMapper(connectionString));
builder.Services.AddScoped<IRefundQuery, RefundManagement>();
builder.Services.AddScoped<IRefundDetails, RefundDetails>();
builder.Services.AddScoped<IOrder, MockOrderService>(); // change later when team5 is done
builder.Services.AddScoped<ISubmitRefund, RefundManagement>();

// Adding services for Support Ticket
builder.Services.AddScoped<ISupportTicket, SupportTicketManagement>();
builder.Services.AddScoped<iSupportTicketQuery, SupportTicketManagement>();
builder.Services.AddScoped<SupportTicketManagement>();
builder.Services.AddScoped<SupportTicketTableDataGateway>(provider =>
    new SupportTicketTableDataGateway(connectionString!));


// This is where I add all the interfaces other users can use
builder.Services.AddScoped<IStaffAuthentication, StaffAuthentication>();

var app = builder.Build();

app.UseSession();

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
    pattern: "{controller=StaffLogin}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
