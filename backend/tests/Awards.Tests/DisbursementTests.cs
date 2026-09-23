using Awards.Web.Models;
using Xunit;

namespace Awards.Tests;

public class DisbursementTests
{
    [Fact]
    public void Outstanding_IsAmountMinusPaid()
    {
        var disbursement = new Disbursement { Amount = 3000m, AmountPaid = 1200m };

        Assert.Equal(1800m, disbursement.Outstanding);
    }

    [Fact]
    public void IsPaid_RequiresDateAndFullAmount()
    {
        var unpaid = new Disbursement { Amount = 3000m, AmountPaid = 3000m };
        var paid = new Disbursement
        {
            Amount = 3000m, AmountPaid = 3000m, PaidOn = new DateTime(2026, 10, 3)
        };

        Assert.False(unpaid.IsPaid);
        Assert.True(paid.IsPaid);
    }

    [Theory]
    [InlineData(ApplicationStatus.Awarded, true)]
    [InlineData(ApplicationStatus.Declined, true)]
    [InlineData(ApplicationStatus.UnderReview, false)]
    [InlineData(ApplicationStatus.Submitted, false)]
    public void IsDecided_ReflectsTerminalStatuses(ApplicationStatus status, bool expected)
    {
        Assert.Equal(expected, new Application { Status = status }.IsDecided);
    }
}
