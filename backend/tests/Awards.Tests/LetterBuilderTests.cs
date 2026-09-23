using Awards.Web.Models;
using Awards.Web.Services;
using Xunit;

namespace Awards.Tests;

public class LetterBuilderTests
{
    private static (Application, Applicant, Programme) Fixture()
    {
        var applicant = new Applicant
        {
            FullName = "Imani Brooks", Email = "imani.brooks@example.edu",
            Institution = "Northfield University"
        };
        var programme = new Programme
        {
            Name = "Access and Opportunity Grant", Code = "CMG-ACCESS",
            Category = ProgrammeCategory.Need, AwardAmount = 9000m
        };
        var application = new Application
        {
            Reference = "CMG-2026-0001", SubmittedOn = new DateTime(2026, 9, 1),
            EligibilityScore = 84.5m, Status = ApplicationStatus.Awarded,
            Disbursements =
            {
                new Disbursement { ScheduledFor = new DateTime(2026, 10, 1), Amount = 3000m, AmountPaid = 3000m },
                new Disbursement { ScheduledFor = new DateTime(2027, 2, 1), Amount = 3000m, AmountPaid = 0m }
            }
        };
        return (application, applicant, programme);
    }

    [Fact]
    public void AwardLetter_CarriesHeaderAndReference()
    {
        var (application, applicant, programme) = Fixture();

        var output = new AwardLetterBuilder().Build(application, applicant, programme);

        Assert.Contains("SCHOLARSHIP AWARD LETTER", output);
        Assert.Contains("CMG-2026-0001", output);
        Assert.Contains("successful", output);
    }

    [Fact]
    public void DeclineLetter_CarriesDecisionWording()
    {
        var (application, applicant, programme) = Fixture();

        var output = new DeclineLetterBuilder().Build(application, applicant, programme);

        Assert.Contains("SCHOLARSHIP DECISION LETTER", output);
        Assert.Contains("not successful", output);
    }

    [Fact]
    public void BothLetters_ReportTheSameOutstandingTotal()
    {
        var (application, applicant, programme) = Fixture();

        var award = new AwardLetterBuilder().Build(application, applicant, programme);
        var decline = new DeclineLetterBuilder().Build(application, applicant, programme);

        Assert.Contains("Outstanding : 3,000.00", award);
        Assert.Contains("Outstanding : 3,000.00", decline);
    }
}
