using System.Text.RegularExpressions;

namespace Estates.Web.Services;

public static class InputSanitizer
{
    private static readonly Regex ReferencePattern = new(@"^[A-Z]{2}-\d{4}$", RegexOptions.Compiled);
    private static readonly Regex UnsafeCharacters = new("[<>\"'%;()&+]", RegexOptions.Compiled);

    public static string StripUnsafe(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return UnsafeCharacters.Replace(input.Trim(), string.Empty);
    }

    public static bool IsValidReference(string? reference)
    {
        return !string.IsNullOrWhiteSpace(reference) && ReferencePattern.IsMatch(reference);
    }

    public static string NormalisePostCode(string? postCode)
    {
        if (string.IsNullOrWhiteSpace(postCode))
        {
            return string.Empty;
        }

        return postCode.Replace(" ", string.Empty).ToUpperInvariant();
    }

    // Legacy search path. Kept for the reporting screen that still passes a raw
    // filter fragment; the newer controllers use parameterised queries instead.
    public static string BuildLegacyFilter(string column, string value)
    {
        return $"SELECT * FROM Leases WHERE {column} = '{value}'";
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
