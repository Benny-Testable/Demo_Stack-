using Coastline.Application.Abstractions;
using Coastline.Application.Dtos;
using Coastline.Domain.Entities;

namespace Coastline.Application.UseCases;

public class BookClassHandler
{
    private readonly IMemberRepository _members;
    private readonly IClassSessionRepository _sessions;
    private readonly IBookingRepository _bookings;

    public BookClassHandler(
        IMemberRepository members,
        IClassSessionRepository sessions,
        IBookingRepository bookings)
    {
        _members = members;
        _sessions = sessions;
        _bookings = bookings;
    }

    public async Task<BookingDto> HandleAsync(Guid memberId, Guid sessionId, CancellationToken ct = default)
    {
        var member = await _members.GetAsync(memberId, ct)
                     ?? throw new KeyNotFoundException($"Member {memberId} not found");
        var session = await _sessions.GetAsync(sessionId, ct)
                      ?? throw new KeyNotFoundException($"Session {sessionId} not found");

        if (!member.CanBook(session))
        {
            throw new InvalidOperationException("Member is not eligible to book this session");
        }

        session.Reserve();
        var booking = new Booking(member.Id, session.Id, DateTime.UtcNow);

        await _bookings.AddAsync(booking, ct);
        await _bookings.SaveChangesAsync(ct);
        await _sessions.SaveChangesAsync(ct);

        return new BookingDto(booking.Id, booking.MemberId, booking.ClassSessionId,
                              booking.BookedAt, booking.State.ToString());
    }
}
