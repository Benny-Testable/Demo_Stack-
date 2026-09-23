namespace Estates.Web.Models.ViewModels;

public class PortfolioSummaryViewModel
{
    public int PropertyCount { get; set; }
    public int ActiveLeaseCount { get; set; }
    public decimal TotalAnnualRent { get; set; }
    public decimal TotalOutstanding { get; set; }
    public IReadOnlyList<PropertyRow> Properties { get; set; } = Array.Empty<PropertyRow>();

    public class PropertyRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int BlockNumber { get; set; }
        public int LeaseCount { get; set; }
        public decimal AnnualRent { get; set; }
        public decimal OccupancyPct { get; set; }
    }
}
