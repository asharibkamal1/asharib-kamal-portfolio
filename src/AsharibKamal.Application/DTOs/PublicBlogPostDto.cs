namespace AsharibKamal.Application.DTOs;

public sealed record PublicBlogPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string Content,
    string Category,
    IReadOnlyList<string> Tags,
    bool IsFeatured,
    DateTime? PublishedAtUtc,
    string ReadTime);
