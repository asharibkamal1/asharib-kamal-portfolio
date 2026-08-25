using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Abstractions;

public interface IPublicBlogService
{
    Task<IReadOnlyList<PublicBlogPostDto>> GetPublishedAsync(CancellationToken cancellationToken = default);
    Task<PublicBlogPostDto?> GetPublishedBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
