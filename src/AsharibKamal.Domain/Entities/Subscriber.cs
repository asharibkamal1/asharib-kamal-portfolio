using AsharibKamal.Domain.Common;

namespace AsharibKamal.Domain.Entities;

public sealed class Subscriber : Entity
{
    public string Email { get; set; } = string.Empty;
    public string Source { get; set; } = "Website";
    public bool IsActive { get; set; } = true;
}
