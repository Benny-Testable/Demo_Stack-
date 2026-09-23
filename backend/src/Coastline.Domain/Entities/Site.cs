namespace Coastline.Domain.Entities;

public class Site
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public int StudioCount { get; private set; }
    public bool HasPool { get; private set; }

    private Site() { }

    public Site(string name, string city, string state, int studioCount, bool hasPool)
    {
        Name = name;
        City = city;
        State = state;
        StudioCount = studioCount;
        HasPool = hasPool;
    }
}
