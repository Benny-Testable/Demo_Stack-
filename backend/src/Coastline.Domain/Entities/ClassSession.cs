namespace Coastline.Domain.Entities;

public class ClassSession
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public string Instructor { get; private set; } = string.Empty;
    public Guid SiteId { get; private set; }
    public DateTime StartsAt { get; private set; }
    public int DurationMinutes { get; private set; }
    public int Capacity { get; private set; }
    public int BookedCount { get; private set; }
    public MembershipTier MinimumTier { get; private set; } = MembershipTier.Flex;

    public bool IsFull => BookedCount >= Capacity;
    public int RemainingPlaces => Math.Max(0, Capacity - BookedCount);
    public DateTime EndsAt => StartsAt.AddMinutes(DurationMinutes);

    private ClassSession() { }

    public ClassSession(string title, string instructor, Guid siteId,
                        DateTime startsAt, int durationMinutes, int capacity,
                        MembershipTier minimumTier = MembershipTier.Flex)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive");
        }
        if (durationMinutes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(durationMinutes), "Duration must be positive");
        }

        Title = title;
        Instructor = instructor;
        SiteId = siteId;
        StartsAt = startsAt;
        DurationMinutes = durationMinutes;
        Capacity = capacity;
        MinimumTier = minimumTier;
    }

    public void Reserve()
    {
        if (IsFull)
        {
            throw new InvalidOperationException("Session is already full");
        }
        BookedCount++;
    }

    public void Release()
    {
        if (BookedCount > 0)
        {
            BookedCount--;
        }
    }
}
