using Coastline.Application.Services;
using Coastline.Domain.Entities;
using Xunit;

namespace Coastline.Tests;

public class StatementBuilderTests
{
    private static (Member, Site, List<Booking>) Fixture()
    {
        var member = new Member("Dana Reyes", "dana@example.com",
                                MembershipTier.Premium, new DateOnly(2023, 5, 1));
        var site = new Site("Coastline Central", "Phoenix", "AZ", 5, true);
        var sessionId = Guid.NewGuid();

        var attended = new Booking(member.Id, sessionId, new DateTime(2026, 9, 1, 7, 0, 0));
        attended.MarkAttended(new DateTime(2026, 9, 1, 7, 5, 0));
        var cancelled = new Booking(member.Id, sessionId, new DateTime(2026, 9, 8, 7, 0, 0));
        cancelled.Cancel();
        var noShow = new Booking(member.Id, sessionId, new DateTime(2026, 9, 15, 7, 0, 0));
        noShow.MarkNoShow();

        return (member, site, new List<Booking> { attended, cancelled, noShow });
    }

    [Fact]
    public void AttendanceStatement_ReportsTotalsAndHeader()
    {
        var (member, site, bookings) = Fixture();

        var output = new AttendanceStatementBuilder().Build(member, site, bookings);

        Assert.Contains("MEMBER ATTENDANCE STATEMENT", output);
        Assert.Contains("Total     : 3", output);
        Assert.Contains("Attended  : 1", output);
        Assert.Contains("Attendance : 33.3%", output);
    }

    [Fact]
    public void EngagementStatement_ReportsSameTotals()
    {
        var (member, site, bookings) = Fixture();

        var output = new EngagementStatementBuilder().Build(member, site, bookings);

        Assert.Contains("MEMBER ENGAGEMENT SUMMARY", output);
        Assert.Contains("Engagement : 33.3%", output);
    }

    [Fact]
    public void Statement_HandlesNoBookings()
    {
        var (member, site, _) = Fixture();

        var output = new AttendanceStatementBuilder().Build(member, site, new List<Booking>());

        Assert.Contains("Total     : 0", output);
        Assert.Contains("Attendance : 0%", output);
    }
}
