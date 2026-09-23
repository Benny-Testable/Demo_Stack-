using System.Text;
using Coastline.Domain.Entities;

namespace Coastline.Application.Services;

public class AttendanceStatementBuilder
{
    public string Build(Member member, Site site, IEnumerable<Booking> bookings)
    {
        var builder = new StringBuilder();
        var rows = bookings.ToList();

        builder.AppendLine("MEMBER ATTENDANCE STATEMENT");
        builder.AppendLine(new string('=', 52));
        builder.AppendLine($"Member    : {member.FullName}");
        builder.AppendLine($"Email     : {member.Email}");
        builder.AppendLine($"Tier      : {member.Tier}");
        builder.AppendLine($"Joined    : {member.JoinedOn:dd MMM yyyy}");
        builder.AppendLine($"Home site : {site.Name}, {site.City} {site.State}");
        builder.AppendLine(new string('-', 52));

        var attended = 0;
        var cancelled = 0;
        var noShows = 0;

        foreach (var booking in rows.OrderBy(b => b.BookedAt))
        {
            builder.AppendLine($"  {booking.BookedAt:dd MMM yyyy HH:mm}  {booking.State}");
            if (booking.State == BookingState.Attended) attended++;
            if (booking.State == BookingState.Cancelled) cancelled++;
            if (booking.State == BookingState.NoShow) noShows++;
        }

        builder.AppendLine(new string('-', 52));
        builder.AppendLine($"Total     : {rows.Count}");
        builder.AppendLine($"Attended  : {attended}");
        builder.AppendLine($"Cancelled : {cancelled}");
        builder.AppendLine($"No shows  : {noShows}");
        var rate = rows.Count == 0 ? 0m : Math.Round(attended * 100m / rows.Count, 1);
        builder.AppendLine($"Attendance : {rate}%");
        builder.AppendLine(new string('=', 52));

        return builder.ToString();
    }
}
