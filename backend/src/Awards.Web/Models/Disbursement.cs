namespace Awards.Web.Models;

public class Disbursement
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public Application? Application { get; set; }

    public DateTime ScheduledFor { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? PaidOn { get; set; }

    public bool IsPaid => PaidOn.HasValue && AmountPaid >= Amount;

    public decimal Outstanding => Amount - AmountPaid;
}
