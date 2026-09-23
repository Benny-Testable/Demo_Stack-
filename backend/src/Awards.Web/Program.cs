using Awards.Web.Data;
using Awards.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AwardsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AwardsDb")));

builder.Services.AddScoped<LegacyEligibilityScorer>();
builder.Services.AddScoped<AwardLetterBuilder>();
builder.Services.AddScoped<DeclineLetterBuilder>();

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
    SeedData.Populate(scope.ServiceProvider.GetRequiredService<AwardsDbContext>());
}

app.Run();

public partial class Program { }
