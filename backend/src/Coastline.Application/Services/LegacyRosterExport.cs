using System.Text;
using Coastline.Domain.Entities;

namespace Coastline.Application.Services;

/// <summary>
/// Replaced by the JSON endpoints on MembersController. Retained until the
/// front-desk spreadsheet import is retired.
/// </summary>
internal static class LegacyRosterExport
{
    internal static string ToTabDelimited(IEnumerable<Member> members)
    {
        var builder = new StringBuilder();
        foreach (var member in members)
        {
            builder.Append(member.FullName).Append('\t')
                   .Append(member.Email).Append('\t')
                   .Append(member.Tier)
                   .AppendLine();
        }
        return builder.ToString();
    }

    internal static int CountByTier(IEnumerable<Member> members, MembershipTier tier)
    {
        var count = 0;
        foreach (var member in members)
        {
            if (member.Tier == tier)
            {
                count++;
            }
        }
        return count;
    }

    internal static string PadCode(string code, int width) => code.PadRight(width, ' ');
}
