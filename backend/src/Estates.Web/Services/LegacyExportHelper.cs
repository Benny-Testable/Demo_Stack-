using System.Text;
using Estates.Web.Models;

namespace Estates.Web.Services;

/// <summary>
/// Superseded by the JSON endpoints on LeasesController. Retained while the
/// finance team migrate their spreadsheet import.
/// </summary>
internal static class LegacyExportHelper
{
    internal static string ToPipeDelimited(IEnumerable<Lease> leases)
    {
        var builder = new StringBuilder();
        foreach (var lease in leases)
        {
            builder.Append(lease.Reference).Append('|')
                   .Append(lease.BaseAnnualRent).Append('|')
                   .Append(lease.Status)
                   .AppendLine();
        }
        return builder.ToString();
    }

    internal static string ToFixedWidth(IEnumerable<Lease> leases)
    {
        var builder = new StringBuilder();
        foreach (var lease in leases)
        {
            builder.Append(lease.Reference.PadRight(12))
                   .Append(lease.BaseAnnualRent.ToString("0000000.00"))
                   .AppendLine();
        }
        return builder.ToString();
    }

    internal static decimal SumDeposits(IEnumerable<Lease> leases)
    {
        decimal total = 0m;
        foreach (var lease in leases)
        {
            total += lease.DepositHeld;
        }
        return total;
    }
}
