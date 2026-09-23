using Coastline.Domain.Entities;
using Xunit;

namespace Coastline.Tests;

public class BookingTests
{
    private static Booking NewBooking() =>
        new(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2026, 9, 20, 18, 0, 0));

    [Fact]
    public void MarkAttended_SetsStateAndTimestamp()
    {
        var booking = NewBooking();
        booking.MarkAttended(new DateTime(2026, 9, 21, 7, 5, 0));

        Assert.Equal(BookingState.Attended, booking.State);
        Assert.NotNull(booking.AttendedAt);
    }

    [Fact]
    public void Cancel_RejectsAttendedBooking()
    {
        var booking = NewBooking();
        booking.MarkAttended(DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => booking.Cancel());
    }

    [Fact]
    public void MarkAttended_RejectsCancelledBooking()
    {
        var booking = NewBooking();
        booking.Cancel();

        Assert.Throws<InvalidOperationException>(() => booking.MarkAttended(DateTime.UtcNow));
    }

    [Fact]
    public void MarkNoShow_OnlyAppliesToReserved()
    {
        var reserved = NewBooking();
        reserved.MarkNoShow();

        var cancelled = NewBooking();
        cancelled.Cancel();
        cancelled.MarkNoShow();

        Assert.Equal(BookingState.NoShow, reserved.State);
        Assert.Equal(BookingState.Cancelled, cancelled.State);
    }
}
