namespace Estates.Web.Models.ViewModels;

public class LeaseDetailViewModel
{
    public Lease Lease { get; set; } = new();
    public string PropertyName { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public decimal CalculatedRent { get; set; }
    public decimal OutstandingBalance { get; set; }
    public IReadOnlyList<PaymentSchedule> UpcomingPayments { get; set; } = Array.Empty<PaymentSchedule>();
}
