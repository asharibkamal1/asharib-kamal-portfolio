using AsharibKamal.Domain.Common;

namespace AsharibKamal.Domain.Entities;

public sealed class ContactMessage : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
}
