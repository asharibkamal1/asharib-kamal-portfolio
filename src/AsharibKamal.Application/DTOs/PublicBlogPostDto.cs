namespace AsharibKamal.Application.DTOs;

public sealed record PublicBlogPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string Content,
    string Category,
    IReadOnlyList<string> Tags,
    string? CoverImageUrl,
    string? SeoTitle,
    string? SeoDescription,
    bool IsFeatured,
    DateTime? PublishedAtUtc,
    string ReadTime);
