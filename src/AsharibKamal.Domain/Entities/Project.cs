using AsharibKamal.Domain.Common;

namespace AsharibKamal.Domain.Entities;

public sealed class Project : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Technologies { get; set; } = [];
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
}
