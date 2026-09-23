using Coastline.Domain.Entities;
using Xunit;

namespace Coastline.Tests;

public class ClassSessionTests
{
    private static ClassSession NewSession(int capacity = 3) =>
        new("HIIT", "Morgan", Guid.NewGuid(), new DateTime(2026, 10, 1, 7, 0, 0), 45, capacity);

    [Fact]
    public void Constructor_RejectsNonPositiveCapacity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ClassSession("HIIT", "Morgan", Guid.NewGuid(), DateTime.UtcNow, 45, 0));
    }

    [Fact]
    public void Constructor_RejectsNonPositiveDuration()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ClassSession("HIIT", "Morgan", Guid.NewGuid(), DateTime.UtcNow, 0, 10));
    }

    [Fact]
    public void Reserve_FillsAndThenThrows()
    {
        var session = NewSession(2);
        session.Reserve();
        session.Reserve();

        Assert.True(session.IsFull);
        Assert.Equal(0, session.RemainingPlaces);
        Assert.Throws<InvalidOperationException>(() => session.Reserve());
    }

    [Fact]
    public void Release_FreesAPlaceAndFloorsAtZero()
    {
        var session = NewSession();
        session.Reserve();
        session.Release();
        session.Release();

        Assert.Equal(3, session.RemainingPlaces);
    }

    [Fact]
    public void EndsAt_AddsDuration()
    {
        Assert.Equal(new DateTime(2026, 10, 1, 7, 45, 0), NewSession().EndsAt);
    }
}
