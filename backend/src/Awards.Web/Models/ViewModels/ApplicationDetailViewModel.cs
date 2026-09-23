namespace Awards.Web.Models.ViewModels;

public class ApplicationDetailViewModel
{
    public Application Application { get; set; } = new();
    public string ApplicantName { get; set; } = string.Empty;
    public string ProgrammeName { get; set; } = string.Empty;
    public decimal RecalculatedScore { get; set; }
    public decimal OutstandingDisbursement { get; set; }
    public IReadOnlyList<Disbursement> UpcomingDisbursements { get; set; } = Array.Empty<Disbursement>();
}
