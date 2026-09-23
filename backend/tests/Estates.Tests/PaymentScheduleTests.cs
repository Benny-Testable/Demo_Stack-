using Estates.Web.Models;
using Xunit;

namespace Estates.Tests;

public class PaymentScheduleTests
{
    [Fact]
    public void Outstanding_IsDueMinusPaid()
    {
        var payment = new PaymentSchedule { AmountDue = 1000m, AmountPaid = 250m };

        Assert.Equal(750m, payment.Outstanding);
    }

    [Fact]
    public void IsSettled_RequiresSettlementDateAndFullPayment()
    {
        var unpaid = new PaymentSchedule { AmountDue = 1000m, AmountPaid = 1000m };
        var settled = new PaymentSchedule
        {
            AmountDue = 1000m, AmountPaid = 1000m, SettledOn = new DateTime(2024, 1, 1)
        };

        Assert.False(unpaid.IsSettled);
        Assert.True(settled.IsSettled);
    }

    [Fact]
    public void TermMonths_CountsWholeMonths()
    {
        var lease = new Lease
        {
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2027, 1, 1)
        };

        Assert.Equal(60, lease.TermMonths);
    }
}
