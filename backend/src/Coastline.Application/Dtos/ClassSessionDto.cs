namespace Coastline.Application.Dtos;

public record ClassSessionDto(
    Guid Id,
    string Title,
    string Instructor,
    DateTime StartsAt,
    int DurationMinutes,
    int Capacity,
    int RemainingPlaces,
    string MinimumTier);

public record BookingDto(
    Guid Id,
    Guid MemberId,
    Guid ClassSessionId,
    DateTime BookedAt,
    string State);
