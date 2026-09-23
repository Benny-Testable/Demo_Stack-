using System.Text.RegularExpressions;

namespace Coastline.Application.Services;

public static class MemberInputSanitizer
{
    private static readonly Regex EmailPattern =
        new(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
    private static readonly Regex Unsafe =
        new("[<>\"'%;()&+]", RegexOptions.Compiled);

    public static string Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : Unsafe.Replace(value.Trim(), string.Empty);

    public static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) && EmailPattern.IsMatch(email);

    public static string NormaliseName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }
        return string.Join(' ', name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    // Reporting screen still passes a raw column fragment for the ad-hoc member
    // search; the use-case handlers use parameterised queries.
    public static string BuildAdHocQuery(string column, string value)
    {
        return $"SELECT * FROM members WHERE {column} = '{value}'";
    }
}
