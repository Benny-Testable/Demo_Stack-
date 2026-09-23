namespace Coastline.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateOnly JoinedOn { get; private set; }
    public MembershipTier Tier { get; private set; } = MembershipTier.Flex;
    public bool IsSuspended { get; private set; }
    public int LoyaltyMonths { get; private set; }

    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    private Member() { }

    public Member(string fullName, string email, MembershipTier tier, DateOnly joinedOn)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Member name is required", nameof(fullName));
        }
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("A valid email is required", nameof(email));
        }

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        Tier = tier;
        JoinedOn = joinedOn;
    }

    public void Suspend() => IsSuspended = true;

    public void Reinstate() => IsSuspended = false;

    public void UpgradeTo(MembershipTier tier)
    {
        if (tier < Tier)
        {
            throw new InvalidOperationException("Use Downgrade to move to a lower tier");
        }
        Tier = tier;
    }

    public void RecordLoyaltyMonth() => LoyaltyMonths++;

    public bool CanBook(ClassSession session)
    {
        if (IsSuspended)
        {
            return false;
        }
        if (session.IsFull)
        {
            return false;
        }
        return session.MinimumTier <= Tier;
    }

    internal void AttachBooking(Booking booking) => _bookings.Add(booking);
}

public enum MembershipTier
{
    Flex = 0,
    Standard = 1,
    Premium = 2,
    Elite = 3
}
