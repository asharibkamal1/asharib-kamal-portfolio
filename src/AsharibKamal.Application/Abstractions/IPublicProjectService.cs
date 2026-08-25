using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Abstractions;

public interface IPublicProjectService
{
    Task<IReadOnlyList<PublicProjectDto>> GetPublishedAsync(CancellationToken cancellationToken = default);
    Task<PublicProjectDto?> GetPublishedBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
