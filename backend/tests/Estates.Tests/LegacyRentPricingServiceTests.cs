using Estates.Web.Models;
using Estates.Web.Services;
using Xunit;

namespace Estates.Tests;

public class LegacyRentPricingServiceTests
{
    private readonly LegacyRentPricingService _service = new();

    private static Property Property(PropertyGrade grade, string city = "Bristol",
                                     int units = 20, decimal area = 20000m) => new()
    {
        Name = "Test Block", City = city, BlockNumber = 1,
        TotalUnits = units, FloorAreaSqFt = area, Grade = grade
    };

    private static Tenant Tenant(int credit = 700, int years = 5, bool anchor = false) => new()
    {
        CompanyName = "Test Co", ContactEmail = "test@example.com",
        CreditScore = credit, YearsTrading = years, IsAnchorTenant = anchor
    };

    private static Lease Lease(decimal rent = 100000m, int units = 5) => new()
    {
        Reference = "TB-0001", BaseAnnualRent = rent, UnitCount = units,
        StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2026, 1, 1),
        Status = LeaseStatus.Active
    };

    [Fact]
    public void ReturnsZero_WhenLeaseIsNull()
    {
        Assert.Equal(0m, _service.CalculateAnnualRent(null!, Property(PropertyGrade.Standard), Tenant()));
    }

    [Fact]
    public void ReturnsZero_WhenLeaseTerminated()
    {
        var lease = Lease();
        lease.Status = LeaseStatus.Terminated;

        Assert.Equal(0m, _service.CalculateAnnualRent(lease, Property(PropertyGrade.Premium), Tenant()));
    }

    [Fact]
    public void AppliesFlagshipUplift()
    {
        var result = _service.CalculateAnnualRent(
            Lease(), Property(PropertyGrade.Flagship), Tenant());

        Assert.True(result > 100000m);
    }

    [Fact]
    public void AppliesAnchorTenantDiscount_OnLargeFlagshipLease()
    {
        var property = Property(PropertyGrade.Flagship);
        var withAnchor = _service.CalculateAnnualRent(Lease(units: 10), property, Tenant(anchor: true));
        var withoutAnchor = _service.CalculateAnnualRent(Lease(units: 10), property, Tenant());

        Assert.True(withAnchor < withoutAnchor);
    }

    [Fact]
    public void AppliesArrearsSurcharge()
    {
        var lease = Lease();
        lease.Status = LeaseStatus.InArrears;

        var arrears = _service.CalculateAnnualRent(lease, Property(PropertyGrade.Standard), Tenant());
        var active = _service.CalculateAnnualRent(Lease(), Property(PropertyGrade.Standard), Tenant());

        Assert.True(arrears > active);
    }

    [Fact]
    public void DiscountsEconomyGrade()
    {
        var result = _service.CalculateAnnualRent(
            Lease(), Property(PropertyGrade.Economy, units: 10), Tenant());

        Assert.True(result < 100000m);
    }

    [Fact]
    public void PenalisesPoorCreditOnStandardGrade()
    {
        var poor = _service.CalculateAnnualRent(Lease(), Property(PropertyGrade.Standard), Tenant(credit: 540));
        var strong = _service.CalculateAnnualRent(Lease(), Property(PropertyGrade.Standard), Tenant(credit: 800));

        Assert.True(poor > strong);
    }

    [Fact]
    public void CalculatesServiceCharge_WithGradeUplift()
    {
        var lease = Lease();
        lease.ServiceCharge = 10000m;

        var premium = _service.CalculateServiceCharge(lease, Property(PropertyGrade.Premium));

        Assert.Equal(11500m, premium);
    }
}
