using Coastline.Domain.Entities;

namespace Coastline.Application.Services;

/// <summary>
/// Monthly dues calculation inherited from the previous club software. The
/// promotion rules have accumulated over several seasons and have never been
/// restructured.
/// </summary>
public class LegacyMembershipPricing
{
    public decimal CalculateMonthlyDues(Member member, Site site, int classesPerMonth, bool isPeakSite)
    {
        decimal dues = member.Tier switch
        {
            MembershipTier.Elite => 189m,
            MembershipTier.Premium => 139m,
            MembershipTier.Standard => 89m,
            _ => 49m
        };

        if (isPeakSite)
        {
            dues *= 1.15m;
            if (site.HasPool)
            {
                dues *= 1.08m;
                if (site.StudioCount > 4)
                {
                    dues *= 1.05m;
                    if (member.Tier == MembershipTier.Elite)
                    {
                        dues *= 0.93m;
                        if (member.LoyaltyMonths > 36)
                        {
                            dues *= 0.95m;
                        }
                        else if (member.LoyaltyMonths > 12)
                        {
                            dues *= 0.98m;
                        }
                    }
                }
                else if (site.StudioCount > 2)
                {
                    dues *= 1.02m;
                }
            }
            else if (site.StudioCount > 6)
            {
                dues *= 1.03m;
            }
        }
        else
        {
            dues *= 0.95m;
            if (site.StudioCount < 3)
            {
                dues *= 0.94m;
                if (member.LoyaltyMonths > 24)
                {
                    dues *= 0.97m;
                }
            }
        }

        if (classesPerMonth > 20)
        {
            dues *= 1.10m;
            if (member.Tier < MembershipTier.Premium)
            {
                dues *= 1.06m;
            }
        }
        else if (classesPerMonth < 4)
        {
            dues *= 0.92m;
            if (member.LoyaltyMonths > 6)
            {
                dues *= 0.98m;
            }
        }

        if (member.IsSuspended)
        {
            return 0m;
        }

        if (member.LoyaltyMonths >= 60)
        {
            dues *= 0.90m;
        }

        return Math.Round(dues, 2);
    }

    public decimal CalculateJoiningFee(Member member, bool waiveForPromotion)
    {
        if (waiveForPromotion)
        {
            return 0m;
        }

        return member.Tier switch
        {
            MembershipTier.Elite => 0m,
            MembershipTier.Premium => 25m,
            _ => 49m
        };
    }
}
