using Awards.Web.Models;

namespace Awards.Web.Data;

public static class SeedData
{
    public static void Populate(AwardsDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Programmes.Any())
        {
            return;
        }

        var programmes = new[]
        {
            new Programme { Name = "Community Merit Award", Code = "CMG-MERIT", Category = ProgrammeCategory.Merit,
                            AwardAmount = 6500m, PlacesAvailable = 20, MinimumGpa = 3.40m,
                            IncomeCeiling = 95000m, ClosesOn = new DateTime(2026, 11, 30) },
            new Programme { Name = "Access and Opportunity Grant", Code = "CMG-ACCESS", Category = ProgrammeCategory.Need,
                            AwardAmount = 9000m, PlacesAvailable = 35, MinimumGpa = 2.80m,
                            IncomeCeiling = 48000m, ClosesOn = new DateTime(2026, 12, 15) },
            new Programme { Name = "Postgraduate Research Fellowship", Code = "CMG-RESEARCH", Category = ProgrammeCategory.Research,
                            AwardAmount = 14500m, PlacesAvailable = 8, MinimumGpa = 3.70m,
                            IncomeCeiling = 120000m, ClosesOn = new DateTime(2026, 10, 31) }
        };
        context.Programmes.AddRange(programmes);
        context.SaveChanges();

        var applicants = new[]
        {
            new Applicant { FullName = "Imani Brooks", Email = "imani.brooks@example.edu",
                            Institution = "Northfield University", YearOfStudy = 3,
                            GradePointAverage = 3.82m, HouseholdIncome = 41000m, Dependants = 2,
                            IsFirstGeneration = true, RegisteredOn = new DateTime(2026, 8, 2) },
            new Applicant { FullName = "Tomas Ferreira", Email = "t.ferreira@example.edu",
                            Institution = "Barrow College", YearOfStudy = 1,
                            GradePointAverage = 3.15m, HouseholdIncome = 88000m, Dependants = 0,
                            RegisteredOn = new DateTime(2026, 8, 19) },
            new Applicant { FullName = "Wen Li", Email = "wen.li@example.edu",
                            Institution = "Northfield University", YearOfStudy = 5,
                            GradePointAverage = 3.91m, HouseholdIncome = 62000m, Dependants = 1,
                            HasPriorAward = true, RegisteredOn = new DateTime(2026, 7, 14) }
        };
        context.Applicants.AddRange(applicants);
        context.SaveChanges();

        var applications = new[]
        {
            new Application { ApplicantId = applicants[0].Id, ProgrammeId = programmes[1].Id,
                              Reference = "CMG-2026-0001", SubmittedOn = new DateTime(2026, 9, 1),
                              EligibilityScore = 84.5m, Status = ApplicationStatus.Awarded,
                              DecidedOn = new DateTime(2026, 9, 12), DecisionNote = "Strong need case with first-generation status." },
            new Application { ApplicantId = applicants[1].Id, ProgrammeId = programmes[0].Id,
                              Reference = "CMG-2026-0002", SubmittedOn = new DateTime(2026, 9, 4),
                              EligibilityScore = 51.0m, Status = ApplicationStatus.Declined,
                              DecidedOn = new DateTime(2026, 9, 14), DecisionNote = "Below the merit threshold for this cycle." },
            new Application { ApplicantId = applicants[2].Id, ProgrammeId = programmes[2].Id,
                              Reference = "CMG-2026-0003", SubmittedOn = new DateTime(2026, 9, 9),
                              EligibilityScore = 79.25m, Status = ApplicationStatus.UnderReview }
        };
        context.Applications.AddRange(applications);
        context.SaveChanges();

        var awarded = applications[0];
        for (var term = 0; term < 3; term++)
        {
            context.Disbursements.Add(new Disbursement
            {
                ApplicationId = awarded.Id,
                ScheduledFor = new DateTime(2026, 10, 1).AddMonths(term * 4),
                Amount = 3000m,
                AmountPaid = term == 0 ? 3000m : 0m,
                PaidOn = term == 0 ? new DateTime(2026, 10, 3) : null
            });
        }
        context.SaveChanges();
    }
}
