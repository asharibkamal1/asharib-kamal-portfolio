using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.DTOs;
using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Services;

public sealed class AdminDashboardService(IDbContextFactory<PortfolioDbContext> dbFactory) : IAdminDashboardService
{
    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var totalSubscribers = await db.Subscribers.CountAsync(cancellationToken);
        var activeSubscribers = await db.Subscribers.CountAsync(x => x.IsActive, cancellationToken);
        var newContactMessages = await db.ContactMessages.CountAsync(x => x.Status == "New", cancellationToken);
        var totalContactMessages = await db.ContactMessages.CountAsync(cancellationToken);

        var recentSubscribers = await db.Subscribers
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(8)
            .Select(x => new AdminSubscriberDto(x.Id, x.Email, x.Source, x.IsActive, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        var recentMessages = await db.ContactMessages
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(8)
            .Select(x => new AdminContactMessageDto(x.Id, x.Name, x.Email, x.Company, x.Subject, x.Message, x.Status, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return new AdminDashboardDto(
            totalSubscribers,
            activeSubscribers,
            newContactMessages,
            totalContactMessages,
            recentSubscribers,
            recentMessages);
    }

    public async Task MarkMessageReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var message = await db.ContactMessages.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (message is null) return;

        message.Status = "Read";
        message.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task SetSubscriberStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var subscriber = await db.Subscribers.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (subscriber is null) return;

        subscriber.IsActive = isActive;
        subscriber.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
    }
}
