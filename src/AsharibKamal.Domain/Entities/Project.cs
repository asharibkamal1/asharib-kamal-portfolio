using AsharibKamal.Domain.Common;

namespace AsharibKamal.Domain.Entities;

public sealed class Project : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Technologies { get; set; } = [];
    public string? CoverImageUrl { get; set; }
    public string? Problem { get; set; }
    public string? Role { get; set; }
    public string? Engineering { get; set; }
    public string? Confidentiality { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
}
