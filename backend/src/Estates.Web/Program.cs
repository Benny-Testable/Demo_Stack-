using Estates.Web.Data;
using Estates.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EstatesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EstatesDb")));

builder.Services.AddScoped<LegacyRentPricingService>();
builder.Services.AddScoped<LeaseReportBuilder>();
builder.Services.AddScoped<InvoiceReportBuilder>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EstatesDbContext>();
    SeedData.Populate(context);
}

app.Run();

public partial class Program { }
