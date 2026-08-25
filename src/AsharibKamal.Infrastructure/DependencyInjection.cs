using AsharibKamal.Application.Abstractions;
using AsharibKamal.Infrastructure.Persistence;
using AsharibKamal.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AsharibKamal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PortfolioDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=AsharibKamalPortfolio;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        }

        services.AddDbContextFactory<PortfolioDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ILeadCaptureService, LeadCaptureService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<IAdminBlogService, AdminBlogService>();
        services.AddScoped<IPublicBlogService, PublicBlogService>();
        services.AddScoped<IAdminProjectService, AdminProjectService>();
        services.AddScoped<IPublicProjectService, PublicProjectService>();

        return services;
    }
}
