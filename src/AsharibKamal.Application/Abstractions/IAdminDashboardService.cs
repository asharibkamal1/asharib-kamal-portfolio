using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Abstractions;

public interface IAdminDashboardService
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task MarkMessageReadAsync(Guid id, CancellationToken cancellationToken = default);
    Task SetSubscriberStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
}
