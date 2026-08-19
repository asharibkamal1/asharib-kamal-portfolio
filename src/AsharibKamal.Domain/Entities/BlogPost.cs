using AsharibKamal.Domain.Common;

namespace AsharibKamal.Domain.Entities;

public sealed class BlogPost : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
}
