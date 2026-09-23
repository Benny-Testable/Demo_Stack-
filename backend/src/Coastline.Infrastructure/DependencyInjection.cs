using Coastline.Application.Abstractions;
using Coastline.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coastline.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CoastlineDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CoastlineDb")));

        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IClassSessionRepository, ClassSessionRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}
