using System.ComponentModel.DataAnnotations;

namespace Estates.Web.Models;

public class Tenant
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string ContactEmail { get; set; } = string.Empty;

    [StringLength(32)]
    public string ContactPhone { get; set; } = string.Empty;

    public int CreditScore { get; set; }

    public int YearsTrading { get; set; }

    public bool IsAnchorTenant { get; set; }

    public DateTime RegisteredOn { get; set; }

    public ICollection<Lease> Leases { get; set; } = new List<Lease>();
}
