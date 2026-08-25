using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.DTOs;
using AsharibKamal.Domain.Entities;
using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Services;

public sealed class PublicProjectService(IDbContextFactory<PortfolioDbContext> dbFactory) : IPublicProjectService
{
    public async Task<IReadOnlyList<PublicProjectDto>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var projects = await db.Projects.AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.SortOrder)
            .ThenBy(x => x.Title)
            .ToListAsync(cancellationToken);
        return projects.Select(Map).ToList();
    }

    public async Task<PublicProjectDto?> GetPublishedBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var project = await db.Projects.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == normalized, cancellationToken);
        return project is null ? null : Map(project);
    }

    private static PublicProjectDto Map(Project x) => new(
        x.Id, x.Title, x.Slug, x.Summary, x.Description, x.Technologies, x.CoverImageUrl,
        x.Problem, x.Role, x.Engineering, x.Confidentiality, x.IsFeatured, x.SortOrder);
}
