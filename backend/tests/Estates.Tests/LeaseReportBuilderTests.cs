using Estates.Web.Models;
using Estates.Web.Services;
using Xunit;

namespace Estates.Tests;

public class LeaseReportBuilderTests
{
    private static (Lease, Property, Tenant) Fixture()
    {
        var property = new Property
        {
            Name = "Harbour Block", AddressLine = "12 Quay Road", City = "Bristol",
            PostCode = "BS1 4TR", BlockNumber = 1, TotalUnits = 18
        };
        var tenant = new Tenant
        {
            CompanyName = "Northgate Legal LLP", ContactEmail = "accounts@northgate.example"
        };
        var lease = new Lease
        {
            Reference = "HB-0001", BaseAnnualRent = 148000m, ServiceCharge = 12400m,
            DepositHeld = 37000m, UnitCount = 6, Status = LeaseStatus.Active,
            StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2027, 1, 1),
            Payments =
            {
                new PaymentSchedule { DueOn = new DateTime(2022, 1, 1), AmountDue = 37000m, AmountPaid = 37000m },
                new PaymentSchedule { DueOn = new DateTime(2022, 4, 1), AmountDue = 37000m, AmountPaid = 0m }
            }
        };
        return (lease, property, tenant);
    }

    [Fact]
    public void LeaseReport_ContainsHeaderAndReference()
    {
        var (lease, property, tenant) = Fixture();

        var output = new LeaseReportBuilder().Build(lease, property, tenant);

        Assert.Contains("LEASE SUMMARY REPORT", output);
        Assert.Contains("HB-0001", output);
        Assert.Contains("Break clause", output);
    }

    [Fact]
    public void InvoiceReport_ContainsHeaderAndRenewalLine()
    {
        var (lease, property, tenant) = Fixture();

        var output = new InvoiceReportBuilder().Build(lease, property, tenant);

        Assert.Contains("RENT INVOICE STATEMENT", output);
        Assert.Contains("Renewal", output);
    }

    [Fact]
    public void BothReports_ReportTheSameOutstandingTotal()
    {
        var (lease, property, tenant) = Fixture();

        var leaseReport = new LeaseReportBuilder().Build(lease, property, tenant);
        var invoiceReport = new InvoiceReportBuilder().Build(lease, property, tenant);

        Assert.Contains("Outstanding : 37,000.00", leaseReport);
        Assert.Contains("Outstanding : 37,000.00", invoiceReport);
    }
}
