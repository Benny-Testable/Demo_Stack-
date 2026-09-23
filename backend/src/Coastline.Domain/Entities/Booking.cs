namespace Coastline.Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid MemberId { get; private set; }
    public Guid ClassSessionId { get; private set; }
    public DateTime BookedAt { get; private set; }
    public BookingState State { get; private set; } = BookingState.Reserved;
    public DateTime? AttendedAt { get; private set; }

    private Booking() { }

    public Booking(Guid memberId, Guid classSessionId, DateTime bookedAt)
    {
        MemberId = memberId;
        ClassSessionId = classSessionId;
        BookedAt = bookedAt;
    }

    public void MarkAttended(DateTime at)
    {
        if (State == BookingState.Cancelled)
        {
            throw new InvalidOperationException("A cancelled booking cannot be attended");
        }
        State = BookingState.Attended;
        AttendedAt = at;
    }

    public void Cancel()
    {
        if (State == BookingState.Attended)
        {
            throw new InvalidOperationException("An attended booking cannot be cancelled");
        }
        State = BookingState.Cancelled;
    }

    public void MarkNoShow()
    {
        if (State == BookingState.Reserved)
        {
            State = BookingState.NoShow;
        }
    }
}

public enum BookingState
{
    Reserved = 0,
    Attended = 1,
    Cancelled = 2,
    NoShow = 3
}
