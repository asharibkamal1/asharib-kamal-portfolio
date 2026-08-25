using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.DTOs;
using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Services;

public sealed class PublicBlogService(IDbContextFactory<PortfolioDbContext> dbFactory) : IPublicBlogService
{
    public async Task<IReadOnlyList<PublicBlogPostDto>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var posts = await db.BlogPosts
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.IsFeatured)
            .ThenByDescending(x => x.PublishedAtUtc)
            .ToListAsync(cancellationToken);

        return posts.Select(Map).ToList();
    }

    public async Task<PublicBlogPostDto?> GetPublishedBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var post = await db.BlogPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == normalizedSlug, cancellationToken);

        return post is null ? null : Map(post);
    }

    private static PublicBlogPostDto Map(Domain.Entities.BlogPost post)
    {
        var wordCount = string.IsNullOrWhiteSpace(post.Content)
            ? 0
            : post.Content.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        var minutes = Math.Max(1, (int)Math.Ceiling(wordCount / 220d));

        return new PublicBlogPostDto(
            post.Id,
            post.Title,
            post.Slug,
            post.Summary,
            post.Content,
            post.Category,
            post.Tags,
            post.IsFeatured,
            post.PublishedAtUtc,
            $"{minutes} min read");
    }
}
