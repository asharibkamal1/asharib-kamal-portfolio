using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Abstractions;

public interface IAdminProjectService
{
    Task<IReadOnlyList<AdminProjectDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AdminProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> SaveAsync(AdminProjectEditDto model, CancellationToken cancellationToken = default);
    Task SetPublishedAsync(Guid id, bool published, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
