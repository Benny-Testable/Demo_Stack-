using Estates.Web.Models;

namespace Estates.Web.Data;

public static class SeedData
{
    public static void Populate(EstatesDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Properties.Any())
        {
            return;
        }

        var properties = new[]
        {
            new Property { Name = "Harbour Block", AddressLine = "12 Quay Road", City = "Bristol",
                           PostCode = "BS1 4TR", BlockNumber = 1, TotalUnits = 18,
                           FloorAreaSqFt = 24500m, Grade = PropertyGrade.Premium,
                           AcquiredOn = new DateTime(2019, 3, 14) },
            new Property { Name = "Foundry Block", AddressLine = "88 Iron Lane", City = "Sheffield",
                           PostCode = "S3 8QT", BlockNumber = 3, TotalUnits = 26,
                           FloorAreaSqFt = 31200m, Grade = PropertyGrade.Standard,
                           AcquiredOn = new DateTime(2020, 9, 2) },
            new Property { Name = "Carlisle Block", AddressLine = "4 Market Street", City = "Leeds",
                           PostCode = "LS1 6DT", BlockNumber = 7, TotalUnits = 12,
                           FloorAreaSqFt = 15800m, Grade = PropertyGrade.Economy,
                           AcquiredOn = new DateTime(2021, 1, 20) }
        };
        context.Properties.AddRange(properties);
        context.SaveChanges();

        var tenants = new[]
        {
            new Tenant { CompanyName = "Northgate Legal LLP", ContactEmail = "accounts@northgate.example",
                         ContactPhone = "0117 496 0021", CreditScore = 812, YearsTrading = 14,
                         IsAnchorTenant = true, RegisteredOn = new DateTime(2019, 6, 1) },
            new Tenant { CompanyName = "Pellow Print Works", ContactEmail = "billing@pellow.example",
                         ContactPhone = "0114 220 8890", CreditScore = 640, YearsTrading = 6,
                         RegisteredOn = new DateTime(2020, 11, 12) },
            new Tenant { CompanyName = "Astra Fit Studios", ContactEmail = "hello@astrafit.example",
                         ContactPhone = "0113 884 1190", CreditScore = 548, YearsTrading = 2,
                         RegisteredOn = new DateTime(2022, 4, 5) }
        };
        context.Tenants.AddRange(tenants);
        context.SaveChanges();

        var leases = new[]
        {
            new Lease { PropertyId = properties[0].Id, TenantId = tenants[0].Id, Reference = "HB-0001",
                        StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2027, 1, 1),
                        BaseAnnualRent = 148000m, ServiceCharge = 12400m, DepositHeld = 37000m,
                        UnitCount = 6, Status = LeaseStatus.Active },
            new Lease { PropertyId = properties[1].Id, TenantId = tenants[1].Id, Reference = "FB-0014",
                        StartDate = new DateTime(2021, 7, 1), EndDate = new DateTime(2026, 7, 1),
                        BaseAnnualRent = 92500m, ServiceCharge = 8800m, DepositHeld = 23125m,
                        UnitCount = 4, HasBreakClause = true, Status = LeaseStatus.InArrears },
            new Lease { PropertyId = properties[2].Id, TenantId = tenants[2].Id, Reference = "CB-0007",
                        StartDate = new DateTime(2023, 2, 1), EndDate = new DateTime(2026, 2, 1),
                        BaseAnnualRent = 41000m, ServiceCharge = 5200m, DepositHeld = 10250m,
                        UnitCount = 2, IsRenewal = true, Status = LeaseStatus.Active }
        };
        context.Leases.AddRange(leases);
        context.SaveChanges();

        foreach (var lease in leases)
        {
            for (var quarter = 0; quarter < 4; quarter++)
            {
                context.PaymentSchedules.Add(new PaymentSchedule
                {
                    LeaseId = lease.Id,
                    DueOn = lease.StartDate.AddMonths(quarter * 3),
                    AmountDue = Math.Round(lease.BaseAnnualRent / 4m, 2),
                    AmountPaid = quarter < 3 ? Math.Round(lease.BaseAnnualRent / 4m, 2) : 0m,
                    SettledOn = quarter < 3 ? lease.StartDate.AddMonths(quarter * 3).AddDays(9) : null
                });
            }
        }
        context.SaveChanges();
    }
}
