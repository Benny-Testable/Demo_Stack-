namespace Coastline.Domain.ValueObjects;

public readonly record struct DateRange(DateOnly Start, DateOnly End)
{
    public int Days => End.DayNumber - Start.DayNumber + 1;

    public bool Contains(DateOnly value) => value >= Start && value <= End;

    public bool Overlaps(DateRange other) => Start <= other.End && other.Start <= End;

    public static DateRange Month(int year, int month)
    {
        var start = new DateOnly(year, month, 1);
        return new DateRange(start, start.AddMonths(1).AddDays(-1));
    }
}
