namespace AsharibKamal.Application.DTOs;

public sealed record ProjectDto(
    string Title,
    string Slug,
    string Summary,
    IReadOnlyList<string> Technologies,
    bool Featured = true);

public sealed record ExperienceDto(
    string Period,
    string Role,
    string Organization,
    string Summary,
    IReadOnlyList<string> Highlights);

public sealed record SkillGroupDto(string Name, IReadOnlyList<string> Skills);

public sealed record BlogPreviewDto(string Title, string Slug, string Summary, string Category, string ReadTime);
