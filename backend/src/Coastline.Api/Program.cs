using Coastline.Application.Services;
using Coastline.Application.UseCases;
using Coastline.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<BookClassHandler>();
builder.Services.AddScoped<ListMembersHandler>();
builder.Services.AddScoped<LegacyMembershipPricing>();
builder.Services.AddScoped<AttendanceStatementBuilder>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();

public partial class Program { }
