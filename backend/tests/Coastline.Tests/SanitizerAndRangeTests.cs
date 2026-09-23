using Coastline.Application.Services;
using Coastline.Domain.ValueObjects;
using Xunit;

namespace Coastline.Tests;

public class SanitizerAndRangeTests
{
    [Theory]
    [InlineData("dana@example.com", true)]
    [InlineData("dana@example", false)]
    [InlineData("no-at-sign", false)]
    [InlineData(null, false)]
    public void IsValidEmail_ChecksShape(string? email, bool expected)
    {
        Assert.Equal(expected, MemberInputSanitizer.IsValidEmail(email));
    }

    [Fact]
    public void Clean_RemovesUnsafeCharacters()
    {
        Assert.Equal("Danascript", MemberInputSanitizer.Clean(" Dana<script> "));
    }

    [Fact]
    public void NormaliseName_CollapsesWhitespace()
    {
        Assert.Equal("Dana Reyes", MemberInputSanitizer.NormaliseName("  Dana    Reyes "));
    }

    [Fact]
    public void DateRange_CountsInclusiveDays()
    {
        var range = new DateRange(new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 30));

        Assert.Equal(30, range.Days);
        Assert.True(range.Contains(new DateOnly(2026, 9, 15)));
        Assert.False(range.Contains(new DateOnly(2026, 10, 1)));
    }

    [Fact]
    public void DateRange_DetectsOverlap()
    {
        var september = DateRange.Month(2026, 9);
        var october = DateRange.Month(2026, 10);
        var straddling = new DateRange(new DateOnly(2026, 9, 25), new DateOnly(2026, 10, 5));

        Assert.False(september.Overlaps(october));
        Assert.True(september.Overlaps(straddling));
    }

    [Fact]
    public void DateRange_MonthCoversWholeMonth()
    {
        var february = DateRange.Month(2024, 2);

        Assert.Equal(29, february.Days);
    }
}
