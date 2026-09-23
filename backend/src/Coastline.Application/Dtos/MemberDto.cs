namespace Coastline.Application.Dtos;

public record MemberDto(
    Guid Id,
    string FullName,
    string Email,
    string Tier,
    DateOnly JoinedOn,
    bool IsSuspended,
    int BookingCount);

public record CreateMemberRequest(string FullName, string Email, string Tier);
