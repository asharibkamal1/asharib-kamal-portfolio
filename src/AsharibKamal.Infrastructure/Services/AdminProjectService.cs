using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.DTOs;
using AsharibKamal.Domain.Entities;
using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Services;

public sealed class AdminProjectService(IDbContextFactory<PortfolioDbContext> dbFactory) : IAdminProjectService
{
    public async Task<IReadOnlyList<AdminProjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var projects = await db.Projects.AsNoTracking()
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Title)
            .ToListAsync(cancellationToken);
        return projects.Select(Map).ToList();
    }

    public async Task<AdminProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var project = await db.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return project is null ? null : Map(project);
    }

    public async Task<Guid> SaveAsync(AdminProjectEditDto model, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var slug = model.Slug.Trim().ToLowerInvariant();
        var duplicate = await db.Projects.AnyAsync(x => x.Slug == slug && (!model.Id.HasValue || x.Id != model.Id.Value), cancellationToken);
        if (duplicate) throw new InvalidOperationException("A project with this slug already exists.");

        Project project;
        if (model.Id.HasValue)
        {
            project = await db.Projects.FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken)
                ?? throw new InvalidOperationException("Project not found.");
        }
        else
        {
            project = new Project();
            db.Projects.Add(project);
        }

        project.Title = model.Title.Trim();
        project.Slug = slug;
        project.Summary = model.Summary.Trim();
        project.Description = model.Description.Trim();
        project.Technologies = model.Technologies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        project.CoverImageUrl = NullIfWhiteSpace(model.CoverImageUrl);
        project.Problem = NullIfWhiteSpace(model.Problem);
        project.Role = NullIfWhiteSpace(model.Role);
        project.Engineering = NullIfWhiteSpace(model.Engineering);
        project.Confidentiality = NullIfWhiteSpace(model.Confidentiality);
        project.IsPublished = model.IsPublished;
        project.IsFeatured = model.IsFeatured;
        project.SortOrder = model.SortOrder;

        await db.SaveChangesAsync(cancellationToken);
        return project.Id;
    }

    public async Task SetPublishedAsync(Guid id, bool published, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null) return;
        project.IsPublished = published;
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null) return;
        db.Projects.Remove(project);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static AdminProjectDto Map(Project x) => new(
        x.Id, x.Title, x.Slug, x.Summary, x.Description, string.Join(", ", x.Technologies), x.CoverImageUrl,
        x.Problem, x.Role, x.Engineering, x.Confidentiality, x.IsPublished, x.IsFeatured, x.SortOrder);

    private static string? NullIfWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
