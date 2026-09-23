using Coastline.Application.Services;
using Coastline.Domain.Entities;
using Xunit;

namespace Coastline.Tests;

public class LegacyMembershipPricingTests
{
    private readonly LegacyMembershipPricing _pricing = new();

    private static Member Member(MembershipTier tier, int loyalty = 0)
    {
        var member = new Member("Dana Reyes", "dana@example.com", tier, new DateOnly(2023, 1, 1));
        for (var i = 0; i < loyalty; i++)
        {
            member.RecordLoyaltyMonth();
        }
        return member;
    }

    private static Site Site(int studios = 3, bool pool = false) =>
        new("Coastline Central", "Phoenix", "AZ", studios, pool);

    [Fact]
    public void SuspendedMember_PaysNothing()
    {
        var member = Member(MembershipTier.Elite);
        member.Suspend();

        Assert.Equal(0m, _pricing.CalculateMonthlyDues(member, Site(), 10, true));
    }

    [Fact]
    public void EliteCostsMoreThanFlex()
    {
        var elite = _pricing.CalculateMonthlyDues(Member(MembershipTier.Elite), Site(), 10, false);
        var flex = _pricing.CalculateMonthlyDues(Member(MembershipTier.Flex), Site(), 10, false);

        Assert.True(elite > flex);
    }

    [Fact]
    public void PeakSiteWithPoolCostsMore()
    {
        var peak = _pricing.CalculateMonthlyDues(Member(MembershipTier.Standard), Site(pool: true), 10, true);
        var offPeak = _pricing.CalculateMonthlyDues(Member(MembershipTier.Standard), Site(), 10, false);

        Assert.True(peak > offPeak);
    }

    [Fact]
    public void HeavyUsageAddsSurchargeForLowerTiers()
    {
        var heavy = _pricing.CalculateMonthlyDues(Member(MembershipTier.Flex), Site(), 25, false);
        var light = _pricing.CalculateMonthlyDues(Member(MembershipTier.Flex), Site(), 10, false);

        Assert.True(heavy > light);
    }

    [Fact]
    public void LightUsageEarnsDiscount()
    {
        var light = _pricing.CalculateMonthlyDues(Member(MembershipTier.Standard, 12), Site(), 2, false);
        var normal = _pricing.CalculateMonthlyDues(Member(MembershipTier.Standard, 12), Site(), 10, false);

        Assert.True(light < normal);
    }

    [Fact]
    public void LongLoyaltyEarnsDiscount()
    {
        var loyal = _pricing.CalculateMonthlyDues(Member(MembershipTier.Premium, 60), Site(), 10, false);
        var newJoiner = _pricing.CalculateMonthlyDues(Member(MembershipTier.Premium), Site(), 10, false);

        Assert.True(loyal < newJoiner);
    }

    [Theory]
    [InlineData(MembershipTier.Elite, 0)]
    [InlineData(MembershipTier.Premium, 25)]
    [InlineData(MembershipTier.Flex, 49)]
    public void JoiningFeeVariesByTier(MembershipTier tier, decimal expected)
    {
        Assert.Equal(expected, _pricing.CalculateJoiningFee(Member(tier), false));
    }

    [Fact]
    public void JoiningFeeWaivedOnPromotion()
    {
        Assert.Equal(0m, _pricing.CalculateJoiningFee(Member(MembershipTier.Flex), true));
    }
}
