using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Abstractions;

public interface IAdminBlogService
{
    Task<IReadOnlyList<AdminBlogPostDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AdminBlogPostDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> SaveAsync(AdminBlogPostEditDto model, CancellationToken cancellationToken = default);
    Task SetPublishedAsync(Guid id, bool published, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
