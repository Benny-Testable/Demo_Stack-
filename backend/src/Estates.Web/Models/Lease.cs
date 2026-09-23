using System.ComponentModel.DataAnnotations;

namespace Estates.Web.Models;

public class Lease
{
    public int Id { get; set; }

    public int PropertyId { get; set; }
    public Property? Property { get; set; }

    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    [StringLength(40)]
    public string Reference { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal BaseAnnualRent { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal DepositHeld { get; set; }

    public int UnitCount { get; set; }
    public bool HasBreakClause { get; set; }
    public bool IsRenewal { get; set; }

    public LeaseStatus Status { get; set; } = LeaseStatus.Draft;

    public ICollection<PaymentSchedule> Payments { get; set; } = new List<PaymentSchedule>();

    public int TermMonths =>
        ((EndDate.Year - StartDate.Year) * 12) + EndDate.Month - StartDate.Month;
}

public enum LeaseStatus
{
    Draft = 0,
    Active = 1,
    InArrears = 2,
    Expired = 3,
    Terminated = 4
}
