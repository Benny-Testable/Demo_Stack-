using Estates.Web.Models;

namespace Estates.Web.Services;

/// <summary>
/// Rent calculation carried over from the original portfolio system. The banding
/// rules have been added to incrementally over several years and have never been
/// restructured.
/// </summary>
public class LegacyRentPricingService
{
    public decimal CalculateAnnualRent(Lease lease, Property property, Tenant tenant)
    {
        if (lease == null || property == null || tenant == null)
        {
            return 0m;
        }

        decimal rent = lease.BaseAnnualRent;

        if (property.Grade == PropertyGrade.Flagship)
        {
            rent *= 1.35m;
            if (property.City == "Bristol" || property.City == "Leeds")
            {
                rent *= 1.08m;
                if (lease.UnitCount > 8)
                {
                    rent *= 1.04m;
                    if (tenant.IsAnchorTenant)
                    {
                        rent *= 0.92m;
                        if (tenant.YearsTrading > 10)
                        {
                            rent *= 0.97m;
                        }
                    }
                }
                else if (lease.UnitCount > 4)
                {
                    rent *= 1.02m;
                }
            }
            else if (property.TotalUnits > 20)
            {
                rent *= 1.03m;
            }
        }
        else if (property.Grade == PropertyGrade.Premium)
        {
            rent *= 1.18m;
            if (lease.TermMonths > 60)
            {
                rent *= 0.94m;
                if (tenant.CreditScore > 750)
                {
                    rent *= 0.96m;
                }
                else if (tenant.CreditScore < 600)
                {
                    rent *= 1.07m;
                    if (lease.DepositHeld < lease.BaseAnnualRent / 4m)
                    {
                        rent *= 1.05m;
                    }
                }
            }
            else if (lease.TermMonths > 24)
            {
                rent *= 0.98m;
            }
        }
        else if (property.Grade == PropertyGrade.Standard)
        {
            if (tenant.CreditScore < 600)
            {
                rent *= 1.06m;
                if (lease.HasBreakClause)
                {
                    rent *= 1.03m;
                }
            }
            else if (tenant.CreditScore > 780)
            {
                rent *= 0.95m;
            }

            if (property.FloorAreaSqFt > 30000m)
            {
                rent *= 1.02m;
            }
        }
        else
        {
            rent *= 0.88m;
            if (property.TotalUnits < 15)
            {
                rent *= 0.96m;
                if (lease.IsRenewal)
                {
                    rent *= 0.98m;
                }
            }
        }

        if (lease.IsRenewal && !lease.HasBreakClause)
        {
            rent *= 0.99m;
        }

        if (lease.Status == LeaseStatus.InArrears)
        {
            rent *= 1.12m;
        }
        else if (lease.Status == LeaseStatus.Terminated)
        {
            return 0m;
        }

        if (rent < 0m)
        {
            rent = 0m;
        }

        return Math.Round(rent, 2);
    }

    public decimal CalculateServiceCharge(Lease lease, Property property)
    {
        decimal charge = lease.ServiceCharge;

        if (property.Grade == PropertyGrade.Flagship || property.Grade == PropertyGrade.Premium)
        {
            charge *= 1.15m;
        }

        if (property.TotalUnits > 24)
        {
            charge *= 0.95m;
        }

        return Math.Round(charge, 2);
    }
}
