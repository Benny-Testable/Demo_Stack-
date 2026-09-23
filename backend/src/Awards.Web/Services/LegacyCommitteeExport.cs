using System.Text;
using Awards.Web.Models;

namespace Awards.Web.Services;

/// <summary>
/// Superseded by the JSON endpoints on ApplicationsController. Retained while
/// the awards committee migrate off their spreadsheet.
/// </summary>
internal static class LegacyCommitteeExport
{
    internal static string ToPipeDelimited(IEnumerable<Application> applications)
    {
        var builder = new StringBuilder();
        foreach (var application in applications)
        {
            builder.Append(application.Reference).Append('|')
                   .Append(application.EligibilityScore).Append('|')
                   .Append(application.Status)
                   .AppendLine();
        }
        return builder.ToString();
    }

    internal static decimal SumAwarded(IEnumerable<Application> applications)
    {
        decimal total = 0m;
        foreach (var application in applications)
        {
            if (application.Status == ApplicationStatus.Awarded && application.Programme is not null)
            {
                total += application.Programme.AwardAmount;
            }
        }
        return total;
    }

    internal static string PadReference(string reference, int width) => reference.PadRight(width, ' ');
}
