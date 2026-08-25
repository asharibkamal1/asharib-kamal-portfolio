using System.ComponentModel.DataAnnotations;

namespace AsharibKamal.Application.DTOs;

public sealed record AdminBlogPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string Content,
    string Category,
    string Tags,
    string? CoverImageUrl,
    string? SeoTitle,
    string? SeoDescription,
    bool IsPublished,
    bool IsFeatured,
    DateTime? PublishedAtUtc);

public sealed class AdminBlogPostEditDto
{
    public Guid? Id { get; set; }

    [Required, StringLength(220)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(220)]
    public string Slug { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Summary { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Url, StringLength(1000)]
    public string? CoverImageUrl { get; set; }

    [StringLength(70)]
    public string? SeoTitle { get; set; }

    [StringLength(170)]
    public string? SeoDescription { get; set; }

    public string Tags { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
}
