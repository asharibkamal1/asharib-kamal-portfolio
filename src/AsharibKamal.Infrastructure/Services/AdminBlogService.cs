using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.DTOs;
using AsharibKamal.Domain.Entities;
using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Services;

public sealed class AdminBlogService(IDbContextFactory<PortfolioDbContext> dbFactory) : IAdminBlogService
{
    public async Task<IReadOnlyList<AdminBlogPostDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        return await db.BlogPosts.AsNoTracking()
            .OrderByDescending(x => x.PublishedAtUtc)
            .ThenBy(x => x.Title)
            .Select(x => new AdminBlogPostDto(x.Id, x.Title, x.Slug, x.Summary, x.Content, x.Category,
                string.Join(", ", x.Tags), x.CoverImageUrl, x.SeoTitle, x.SeoDescription,
                x.IsPublished, x.IsFeatured, x.PublishedAtUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task<AdminBlogPostDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var post = await db.BlogPosts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return post is null ? null : new AdminBlogPostDto(post.Id, post.Title, post.Slug, post.Summary, post.Content,
            post.Category, string.Join(", ", post.Tags), post.CoverImageUrl, post.SeoTitle, post.SeoDescription,
            post.IsPublished, post.IsFeatured, post.PublishedAtUtc);
    }

    public async Task<Guid> SaveAsync(AdminBlogPostEditDto model, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var slug = model.Slug.Trim().ToLowerInvariant();
        var duplicate = await db.BlogPosts.AnyAsync(x => x.Slug == slug && (!model.Id.HasValue || x.Id != model.Id.Value), cancellationToken);
        if (duplicate) throw new InvalidOperationException("A blog post with this slug already exists.");

        BlogPost post;
        if (model.Id.HasValue)
        {
            post = await db.BlogPosts.FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken)
                ?? throw new InvalidOperationException("Blog post not found.");
        }
        else
        {
            post = new BlogPost();
            db.BlogPosts.Add(post);
        }

        post.Title = model.Title.Trim();
        post.Slug = slug;
        post.Summary = model.Summary.Trim();
        post.Content = model.Content.Trim();
        post.Category = model.Category.Trim();
        post.Tags = model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        post.CoverImageUrl = string.IsNullOrWhiteSpace(model.CoverImageUrl) ? null : model.CoverImageUrl.Trim();
        post.SeoTitle = string.IsNullOrWhiteSpace(model.SeoTitle) ? null : model.SeoTitle.Trim();
        post.SeoDescription = string.IsNullOrWhiteSpace(model.SeoDescription) ? null : model.SeoDescription.Trim();
        post.IsFeatured = model.IsFeatured;
        if (model.IsPublished && !post.IsPublished) post.PublishedAtUtc = DateTime.UtcNow;
        if (!model.IsPublished) post.PublishedAtUtc = null;
        post.IsPublished = model.IsPublished;

        await db.SaveChangesAsync(cancellationToken);
        return post.Id;
    }

    public async Task SetPublishedAsync(Guid id, bool published, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var post = await db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (post is null) return;
        post.IsPublished = published;
        post.PublishedAtUtc = published ? post.PublishedAtUtc ?? DateTime.UtcNow : null;
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var post = await db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (post is null) return;
        db.BlogPosts.Remove(post);
        await db.SaveChangesAsync(cancellationToken);
    }
}
