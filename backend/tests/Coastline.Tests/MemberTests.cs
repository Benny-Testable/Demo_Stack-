using Coastline.Domain.Entities;
using Xunit;

namespace Coastline.Tests;

public class MemberTests
{
    private static Member NewMember(MembershipTier tier = MembershipTier.Standard) =>
        new("Dana Reyes", "Dana.Reyes@example.com", tier, new DateOnly(2023, 5, 1));

    [Fact]
    public void Constructor_NormalisesEmail()
    {
        Assert.Equal("dana.reyes@example.com", NewMember().Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_RejectsEmptyName(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new Member(name, "a@b.com", MembershipTier.Flex, new DateOnly(2024, 1, 1)));
    }

    [Fact]
    public void Constructor_RejectsInvalidEmail()
    {
        Assert.Throws<ArgumentException>(() =>
            new Member("Dana", "not-an-email", MembershipTier.Flex, new DateOnly(2024, 1, 1)));
    }

    [Fact]
    public void UpgradeTo_RejectsLowerTier()
    {
        var member = NewMember(MembershipTier.Premium);
        Assert.Throws<InvalidOperationException>(() => member.UpgradeTo(MembershipTier.Flex));
    }

    [Fact]
    public void CanBook_IsFalseWhenSuspended()
    {
        var member = NewMember();
        member.Suspend();
        var session = new ClassSession("Spin", "Kai", Guid.NewGuid(), DateTime.UtcNow, 45, 20);

        Assert.False(member.CanBook(session));
    }

    [Fact]
    public void CanBook_IsFalseWhenTierTooLow()
    {
        var member = NewMember(MembershipTier.Flex);
        var session = new ClassSession("Reformer", "Sam", Guid.NewGuid(), DateTime.UtcNow,
                                       50, 12, MembershipTier.Premium);

        Assert.False(member.CanBook(session));
    }

    [Fact]
    public void CanBook_IsTrueWhenEligible()
    {
        var member = NewMember(MembershipTier.Premium);
        var session = new ClassSession("Reformer", "Sam", Guid.NewGuid(), DateTime.UtcNow,
                                       50, 12, MembershipTier.Standard);

        Assert.True(member.CanBook(session));
    }

    [Fact]
    public void Reinstate_ClearsSuspension()
    {
        var member = NewMember();
        member.Suspend();
        member.Reinstate();

        Assert.False(member.IsSuspended);
    }
}
