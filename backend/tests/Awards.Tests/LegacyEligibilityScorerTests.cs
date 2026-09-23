using Awards.Web.Models;
using Awards.Web.Services;
using Xunit;

namespace Awards.Tests;

public class LegacyEligibilityScorerTests
{
    private readonly LegacyEligibilityScorer _scorer = new();

    private static Applicant Applicant(decimal gpa = 3.5m, decimal income = 50000m,
                                       int dependants = 0, int year = 3,
                                       bool firstGen = false, bool prior = false) => new()
    {
        FullName = "Test Applicant", Email = "test@example.edu", Institution = "Test College",
        GradePointAverage = gpa, HouseholdIncome = income, Dependants = dependants,
        YearOfStudy = year, IsFirstGeneration = firstGen, HasPriorAward = prior
    };

    private static Programme Programme(ProgrammeCategory category, decimal minGpa = 3.0m,
                                       decimal ceiling = 90000m, int places = 20) => new()
    {
        Name = "Test Programme", Code = "CMG-TEST", Category = category,
        MinimumGpa = minGpa, IncomeCeiling = ceiling, PlacesAvailable = places,
        AwardAmount = 5000m, IsOpen = true
    };

    [Fact]
    public void ReturnsZero_WhenProgrammeClosed()
    {
        var programme = Programme(ProgrammeCategory.Merit);
        programme.IsOpen = false;

        Assert.Equal(0m, _scorer.Score(Applicant(), programme));
    }

    [Fact]
    public void ReturnsZero_WhenGpaBelowMinimum()
    {
        Assert.Equal(0m, _scorer.Score(Applicant(gpa: 2.5m), Programme(ProgrammeCategory.Merit)));
    }

    [Fact]
    public void ReturnsZero_WhenIncomeOverCeilingForNeed()
    {
        var score = _scorer.Score(Applicant(income: 100000m),
                                  Programme(ProgrammeCategory.Need, ceiling: 48000m));

        Assert.Equal(0m, score);
    }

    [Fact]
    public void ReturnsZero_WhenTooJuniorForResearch()
    {
        Assert.Equal(0m, _scorer.Score(Applicant(gpa: 3.9m, year: 2),
                                       Programme(ProgrammeCategory.Research, minGpa: 3.7m)));
    }

    [Fact]
    public void MeritRewardsHighGpa()
    {
        var high = _scorer.Score(Applicant(gpa: 3.95m), Programme(ProgrammeCategory.Merit));
        var modest = _scorer.Score(Applicant(gpa: 3.2m), Programme(ProgrammeCategory.Merit));

        Assert.True(high > modest);
    }

    [Fact]
    public void MeritPenalisesPriorAward()
    {
        var prior = _scorer.Score(Applicant(gpa: 3.95m, year: 4, prior: true),
                                  Programme(ProgrammeCategory.Merit));
        var fresh = _scorer.Score(Applicant(gpa: 3.95m, year: 4),
                                  Programme(ProgrammeCategory.Merit));

        Assert.True(prior < fresh);
    }

    [Fact]
    public void NeedRewardsLowIncomeAndDependants()
    {
        var high = _scorer.Score(Applicant(income: 12000m, dependants: 3, firstGen: true),
                                 Programme(ProgrammeCategory.Need, ceiling: 48000m));
        var low = _scorer.Score(Applicant(income: 46000m),
                                Programme(ProgrammeCategory.Need, ceiling: 48000m));

        Assert.True(high > low);
    }

    [Fact]
    public void ScoreIsCappedAtOneHundred()
    {
        var score = _scorer.Score(Applicant(gpa: 4.0m, income: 1000m, dependants: 5, firstGen: true),
                                  Programme(ProgrammeCategory.Need, minGpa: 2.0m, ceiling: 90000m));

        Assert.True(score <= 100m);
    }

    [Fact]
    public void SmallProgrammesApplyScarcityFactor()
    {
        var scarce = _scorer.Score(Applicant(gpa: 3.8m), Programme(ProgrammeCategory.Merit, places: 5));
        var open = _scorer.Score(Applicant(gpa: 3.8m), Programme(ProgrammeCategory.Merit, places: 40));

        Assert.True(scarce < open);
    }

    [Theory]
    [InlineData(ProgrammeCategory.Research, 70, true)]
    [InlineData(ProgrammeCategory.Research, 69, false)]
    [InlineData(ProgrammeCategory.Merit, 65, true)]
    [InlineData(ProgrammeCategory.Need, 55, true)]
    [InlineData(ProgrammeCategory.General, 49, false)]
    public void MeetsThreshold_VariesByCategory(ProgrammeCategory category, decimal score, bool expected)
    {
        Assert.Equal(expected, _scorer.MeetsThreshold(score, category));
    }
}
