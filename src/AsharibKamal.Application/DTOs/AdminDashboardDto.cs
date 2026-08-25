namespace AsharibKamal.Application.DTOs;

public sealed record AdminDashboardDto(
    int TotalSubscribers,
    int ActiveSubscribers,
    int NewContactMessages,
    int TotalContactMessages,
    IReadOnlyList<AdminSubscriberDto> RecentSubscribers,
    IReadOnlyList<AdminContactMessageDto> RecentMessages);

public sealed record AdminSubscriberDto(
    Guid Id,
    string Email,
    string Source,
    bool IsActive,
    DateTime CreatedAtUtc);

public sealed record AdminContactMessageDto(
    Guid Id,
    string Name,
    string Email,
    string? Company,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAtUtc);
