using InterportCargo.Application.Interfaces;
using InterportCargo.Application.Services;
using InterportCargo.BusinessLogic.Interfaces;
using InterportCargo.BusinessLogic.Services;
using InterportCargo.DataAccess.Interfaces;
using InterportCargo.DataAccess.Repositories;
using InterportCargo.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using InterportCargo.BusinessLogic.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<InterportCargoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register layered architecture dependencies
builder.Services.AddScoped<ICustomerAppService, CustomerAppService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, EFCustomerRepository>();

builder.Services.AddScoped<IEmployeeAppService, EmployeeAppService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EFEmployeeRepository>();

// Register discount service
builder.Services.AddScoped<IDiscountService, DiscountService>();

builder.Services.AddScoped<IQuotationRequestRepository, EFQuotationRequestRepository>();
builder.Services.AddScoped<IQuotationResponseRepository, EFQuotationResponseRepository>();
builder.Services.AddScoped<IQuotationDetailsRepository, EFQuotationDetailsRepository>();
builder.Services.AddScoped<IRateScheduleRepository, EFRateScheduleRepository>();

// Register authentication service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// One-off maintenance: clear quotations when invoked with --clear-quotations
if (args.Contains("--clear-quotations", StringComparer.OrdinalIgnoreCase))
{
    using (var tempApp = builder.Build())
    using (var scope = tempApp.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<InterportCargoDbContext>();

        // Ensure database exists
        context.Database.EnsureCreated();

        // Delete all quotation requests (children cascade), then reset AUTOINCREMENT counters
        context.Database.ExecuteSqlRaw("DELETE FROM \"QuotationRequests\";");
        context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name IN ('QuotationRequests','QuotationResponses','QuotationDetails');");
    }

    Console.WriteLine("Quotation data cleared (requests, responses, details). Exiting.");
    return;
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Seed rate schedule data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InterportCargoDbContext>();
    
    // Check if rate schedules already exist
    if (!context.RateSchedules.Any())
    {
        var rateSchedules = new List<RateSchedule>
        {
            new RateSchedule { ServiceType = "Walf Booking Fee", Rate20Feet = 60m, Rate40Feet = 70m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "Lift On/Lift Off", Rate20Feet = 80m, Rate40Feet = 120m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "Fumigation", Rate20Feet = 220m, Rate40Feet = 280m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "LCL Delivery Depot", Rate20Feet = 400m, Rate40Feet = 500m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "Tailgate Inspection", Rate20Feet = 120m, Rate40Feet = 160m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "Storage Fee", Rate20Feet = 240m, Rate40Feet = 300m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "Facility Fee", Rate20Feet = 70m, Rate40Feet = 100m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "Walf Inspection", Rate20Feet = 60m, Rate40Feet = 90m, IsActive = true, CreatedDate = DateTime.UtcNow },
            new RateSchedule { ServiceType = "GST", Rate20Feet = 10m, Rate40Feet = 10m, Description = "Goods and Services Tax - 10%", IsActive = true, CreatedDate = DateTime.UtcNow }
        };

        context.RateSchedules.AddRange(rateSchedules);
        context.SaveChanges();
    }
}

app.Run();
