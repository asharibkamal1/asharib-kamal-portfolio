using AsharibKamal.Application.Abstractions;
using AsharibKamal.Domain.Entities;
using AsharibKamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Services;

public sealed class LeadCaptureService(IDbContextFactory<PortfolioDbContext> dbFactory) : ILeadCaptureService
{
    public async Task CaptureContactAsync(ContactLeadRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        db.ContactMessages.Add(new ContactMessage
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Company = string.IsNullOrWhiteSpace(request.Company) ? null : request.Company.Trim(),
            Subject = request.Subject.Trim(),
            Message = request.Message.Trim(),
            Status = "New"
        });

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<NewsletterSignupResult> SubscribeAsync(string email, string source = "Website", CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var existing = await db.Subscribers.SingleOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (existing is null)
        {
            db.Subscribers.Add(new Subscriber
            {
                Email = normalizedEmail,
                Source = source,
                IsActive = true
            });
            await db.SaveChangesAsync(cancellationToken);
            return NewsletterSignupResult.Added;
        }

        if (existing.IsActive)
            return NewsletterSignupResult.AlreadySubscribed;

        existing.IsActive = true;
        existing.Source = source;
        await db.SaveChangesAsync(cancellationToken);
        return NewsletterSignupResult.Reactivated;
    }
}
