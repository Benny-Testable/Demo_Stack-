namespace Awards.Web.Models.ViewModels;

public class AwardsSummaryViewModel
{
    public int ProgrammeCount { get; set; }
    public int ApplicationCount { get; set; }
    public int AwardedCount { get; set; }
    public decimal TotalAwarded { get; set; }
    public IReadOnlyList<ProgrammeRow> Programmes { get; set; } = Array.Empty<ProgrammeRow>();

    public class ProgrammeRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Applications { get; set; }
        public int PlacesAvailable { get; set; }
        public decimal AwardAmount { get; set; }
    }
}
