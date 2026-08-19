using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Abstractions;

public interface IPortfolioService
{
    IReadOnlyList<ProjectDto> GetFeaturedProjects();
    IReadOnlyList<ExperienceDto> GetExperience();
    IReadOnlyList<SkillGroupDto> GetSkillGroups();
    IReadOnlyList<BlogPreviewDto> GetLatestArticles();
}
