using System.ComponentModel.DataAnnotations;

namespace AsharibKamal.Application.DTOs;

public sealed record AdminProjectDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string Description,
    string Technologies,
    string? CoverImageUrl,
    string? Problem,
    string? Role,
    string? Engineering,
    string? Confidentiality,
    bool IsPublished,
    bool IsFeatured,
    int SortOrder);

public sealed class AdminProjectEditDto
{
    public Guid? Id { get; set; }
    [Required, StringLength(180)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(180)] public string Slug { get; set; } = string.Empty;
    [Required, StringLength(500)] public string Summary { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    public string Technologies { get; set; } = string.Empty;
    [StringLength(1000)] public string? CoverImageUrl { get; set; }
    [StringLength(1500)] public string? Problem { get; set; }
    [StringLength(1500)] public string? Role { get; set; }
    [StringLength(1500)] public string? Engineering { get; set; }
    [StringLength(1500)] public string? Confidentiality { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    [Range(0, 999)] public int SortOrder { get; set; }
}

public sealed record PublicProjectDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string Description,
    IReadOnlyList<string> Technologies,
    string? CoverImageUrl,
    string? Problem,
    string? Role,
    string? Engineering,
    string? Confidentiality,
    bool IsFeatured,
    int SortOrder);
