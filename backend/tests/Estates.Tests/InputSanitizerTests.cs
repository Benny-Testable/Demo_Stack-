using Estates.Web.Services;
using Xunit;

namespace Estates.Tests;

public class InputSanitizerTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("  Harbour Block  ", "Harbour Block")]
    [InlineData("Drop<script>", "Dropscript")]
    public void StripUnsafe_RemovesDangerousCharacters(string? input, string expected)
    {
        Assert.Equal(expected, InputSanitizer.StripUnsafe(input));
    }

    [Theory]
    [InlineData("HB-0001", true)]
    [InlineData("hb-0001", false)]
    [InlineData("HB-1", false)]
    [InlineData(null, false)]
    public void IsValidReference_MatchesExpectedPattern(string? reference, bool expected)
    {
        Assert.Equal(expected, InputSanitizer.IsValidReference(reference));
    }

    [Theory]
    [InlineData("bs1 4tr", "BS14TR")]
    [InlineData(" S3 8QT ", "S38QT")]
    [InlineData(null, "")]
    public void NormalisePostCode_StripsSpacesAndUppercases(string? input, string expected)
    {
        Assert.Equal(expected, InputSanitizer.NormalisePostCode(input));
    }

    [Fact]
    public void EscapeForDisplay_EncodesMarkup()
    {
        Assert.Equal("&lt;b&gt;x&lt;/b&gt;", InputSanitizer.EscapeForDisplay("<b>x</b>"));
    }
}
