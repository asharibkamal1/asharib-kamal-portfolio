using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AsharibKamal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PortfolioDb");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContextFactory<PortfolioDbContext>(options => options.UseSqlServer(connectionString));
        }

        return services;
    }
}
