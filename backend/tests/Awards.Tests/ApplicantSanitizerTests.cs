using Awards.Web.Services;
using Xunit;

namespace Awards.Tests;

public class ApplicantSanitizerTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("  Imani Brooks  ", "Imani Brooks")]
    [InlineData("Drop<script>", "Dropscript")]
    public void Clean_RemovesUnsafeCharacters(string? input, string expected)
    {
        Assert.Equal(expected, ApplicantSanitizer.Clean(input));
    }

    [Theory]
    [InlineData("CMG-2026-0001", true)]
    [InlineData("cmg-2026-0001", false)]
    [InlineData("CMG-26-1", false)]
    [InlineData(null, false)]
    public void IsValidReference_MatchesPattern(string? reference, bool expected)
    {
        Assert.Equal(expected, ApplicantSanitizer.IsValidReference(reference));
    }

    [Theory]
    [InlineData("  Imani.Brooks@Example.EDU ", "imani.brooks@example.edu")]
    [InlineData(null, "")]
    public void NormaliseEmail_TrimsAndLowercases(string? input, string expected)
    {
        Assert.Equal(expected, ApplicantSanitizer.NormaliseEmail(input));
    }

    [Fact]
    public void EscapeForDisplay_EncodesMarkup()
    {
        Assert.Equal("&lt;b&gt;x&lt;/b&gt;", ApplicantSanitizer.EscapeForDisplay("<b>x</b>"));
    }
}
