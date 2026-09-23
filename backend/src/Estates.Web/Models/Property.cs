using System.ComponentModel.DataAnnotations;

namespace Estates.Web.Models;

public class Property
{
    public int Id { get; set; }

    [Required, StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(240)]
    public string AddressLine { get; set; } = string.Empty;

    [StringLength(80)]
    public string City { get; set; } = string.Empty;

    [StringLength(16)]
    public string PostCode { get; set; } = string.Empty;

    public int BlockNumber { get; set; }

    public int TotalUnits { get; set; }

    public decimal FloorAreaSqFt { get; set; }

    public PropertyGrade Grade { get; set; } = PropertyGrade.Standard;

    public bool IsActive { get; set; } = true;

    public DateTime AcquiredOn { get; set; }

    public ICollection<Lease> Leases { get; set; } = new List<Lease>();
}

public enum PropertyGrade
{
    Economy = 0,
    Standard = 1,
    Premium = 2,
    Flagship = 3
}
