namespace Estates.Web.Models;

public class PaymentSchedule
{
    public int Id { get; set; }

    public int LeaseId { get; set; }
    public Lease? Lease { get; set; }

    public DateTime DueOn { get; set; }
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? SettledOn { get; set; }

    public bool IsSettled => SettledOn.HasValue && AmountPaid >= AmountDue;

    public decimal Outstanding => AmountDue - AmountPaid;
}
