using System.ComponentModel.DataAnnotations;

namespace Awards.Web.Models;

public class Applicant
{
    public int Id { get; set; }

    [Required, StringLength(160)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(120)]
    public string Institution { get; set; } = string.Empty;

    public int YearOfStudy { get; set; }

    public decimal GradePointAverage { get; set; }

    public decimal HouseholdIncome { get; set; }

    public int Dependants { get; set; }

    public bool IsFirstGeneration { get; set; }

    public bool HasPriorAward { get; set; }

    public DateTime RegisteredOn { get; set; }

    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
