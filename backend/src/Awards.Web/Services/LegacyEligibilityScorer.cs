using Awards.Web.Models;

namespace Awards.Web.Services;

/// <summary>
/// Eligibility scoring carried over from the trust's original grants system.
/// The weighting bands have been extended each award cycle and have never been
/// restructured.
/// </summary>
public class LegacyEligibilityScorer
{
    public decimal Score(Applicant applicant, Programme programme)
    {
        if (applicant == null || programme == null)
        {
            return 0m;
        }

        if (!programme.IsOpen)
        {
            return 0m;
        }

        if (applicant.GradePointAverage < programme.MinimumGpa)
        {
            return 0m;
        }

        decimal score = 40m;

        if (programme.Category == ProgrammeCategory.Merit)
        {
            score += (applicant.GradePointAverage - programme.MinimumGpa) * 40m;
            if (applicant.GradePointAverage >= 3.9m)
            {
                score += 12m;
                if (applicant.YearOfStudy >= 3)
                {
                    score += 6m;
                    if (applicant.HasPriorAward)
                    {
                        score -= 8m;
                    }
                    else if (applicant.IsFirstGeneration)
                    {
                        score += 5m;
                    }
                }
            }
            else if (applicant.GradePointAverage >= 3.6m)
            {
                score += 6m;
            }
        }
        else if (programme.Category == ProgrammeCategory.Need)
        {
            if (applicant.HouseholdIncome > programme.IncomeCeiling)
            {
                return 0m;
            }

            var headroom = programme.IncomeCeiling - applicant.HouseholdIncome;
            score += Math.Min(30m, headroom / 2000m);

            if (applicant.Dependants > 0)
            {
                score += Math.Min(12m, applicant.Dependants * 4m);
                if (applicant.Dependants >= 3)
                {
                    score += 4m;
                    if (applicant.IsFirstGeneration)
                    {
                        score += 6m;
                    }
                }
            }

            if (applicant.IsFirstGeneration)
            {
                score += 8m;
                if (applicant.HouseholdIncome < programme.IncomeCeiling / 2m)
                {
                    score += 5m;
                }
            }

            if (applicant.HasPriorAward)
            {
                score -= 10m;
            }
        }
        else if (programme.Category == ProgrammeCategory.Research)
        {
            if (applicant.YearOfStudy < 4)
            {
                return 0m;
            }

            score += (applicant.GradePointAverage - programme.MinimumGpa) * 55m;
            if (applicant.YearOfStudy >= 5)
            {
                score += 10m;
                if (applicant.HasPriorAward)
                {
                    score += 4m;
                }
            }
        }
        else
        {
            score += (applicant.GradePointAverage - 2.0m) * 10m;
            if (applicant.IsFirstGeneration)
            {
                score += 4m;
            }
        }

        if (programme.PlacesAvailable < 10)
        {
            score *= 0.95m;
        }

        if (score < 0m)
        {
            score = 0m;
        }

        if (score > 100m)
        {
            score = 100m;
        }

        return Math.Round(score, 2);
    }

    public bool MeetsThreshold(decimal score, ProgrammeCategory category) => category switch
    {
        ProgrammeCategory.Research => score >= 70m,
        ProgrammeCategory.Merit => score >= 65m,
        ProgrammeCategory.Need => score >= 55m,
        _ => score >= 50m
    };
}
