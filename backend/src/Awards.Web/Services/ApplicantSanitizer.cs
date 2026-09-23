using System.Text.RegularExpressions;

namespace Awards.Web.Services;

public static class ApplicantSanitizer
{
    private static readonly Regex ReferencePattern =
        new(@"^CMG-\d{4}-\d{4}$", RegexOptions.Compiled);
    private static readonly Regex Unsafe =
        new("[<>\"'%;()&+]", RegexOptions.Compiled);

    public static string Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : Unsafe.Replace(value.Trim(), string.Empty);

    public static bool IsValidReference(string? reference) =>
        !string.IsNullOrWhiteSpace(reference) && ReferencePattern.IsMatch(reference);

    public static string NormaliseEmail(string? email) =>
        string.IsNullOrWhiteSpace(email) ? string.Empty : email.Trim().ToLowerInvariant();

    // Awards committee export still passes a raw column fragment; the controllers
    // use parameterised queries.
    public static string BuildCommitteeQuery(string column, string value)
    {
        return $"SELECT * FROM Applications WHERE {column} = '{value}'";
    }

    public static string EscapeForDisplay(string? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return value
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;");
    }
}
