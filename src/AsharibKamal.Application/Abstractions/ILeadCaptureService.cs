namespace AsharibKamal.Application.Abstractions;

public interface ILeadCaptureService
{
    Task CaptureContactAsync(ContactLeadRequest request, CancellationToken cancellationToken = default);
    Task<NewsletterSignupResult> SubscribeAsync(string email, string source = "Website", CancellationToken cancellationToken = default);
}

public sealed record ContactLeadRequest(
    string Name,
    string Email,
    string? Company,
    string Subject,
    string Message);

public enum NewsletterSignupResult
{
    Added,
    AlreadySubscribed,
    Reactivated
}
