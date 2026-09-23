using System.ComponentModel.DataAnnotations;

namespace Awards.Web.Models;

public class Programme
{
    public int Id { get; set; }

    [Required, StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [StringLength(24)]
    public string Code { get; set; } = string.Empty;

    public ProgrammeCategory Category { get; set; } = ProgrammeCategory.General;

    public decimal AwardAmount { get; set; }

    public int PlacesAvailable { get; set; }

    public decimal MinimumGpa { get; set; }

    public decimal IncomeCeiling { get; set; }

    public bool IsOpen { get; set; } = true;

    public DateTime ClosesOn { get; set; }

    public ICollection<Application> Applications { get; set; } = new List<Application>();
}

public enum ProgrammeCategory
{
    General = 0,
    Merit = 1,
    Need = 2,
    Research = 3
}
