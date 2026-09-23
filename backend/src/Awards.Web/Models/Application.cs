using System.ComponentModel.DataAnnotations;

namespace Awards.Web.Models;

public class Application
{
    public int Id { get; set; }

    public int ApplicantId { get; set; }
    public Applicant? Applicant { get; set; }

    public int ProgrammeId { get; set; }
    public Programme? Programme { get; set; }

    [StringLength(40)]
    public string Reference { get; set; } = string.Empty;

    public DateTime SubmittedOn { get; set; }

    public decimal EligibilityScore { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

    public DateTime? DecidedOn { get; set; }

    [StringLength(400)]
    public string DecisionNote { get; set; } = string.Empty;

    public ICollection<Disbursement> Disbursements { get; set; } = new List<Disbursement>();

    public bool IsDecided => Status is ApplicationStatus.Awarded or ApplicationStatus.Declined;
}

public enum ApplicationStatus
{
    Submitted = 0,
    UnderReview = 1,
    Awarded = 2,
    Declined = 3,
    Withdrawn = 4
}
