using Coastline.Application.Abstractions;
using Coastline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coastline.Infrastructure.Persistence;

public class ClassSessionRepository : IClassSessionRepository
{
    private readonly CoastlineDbContext _context;

    public ClassSessionRepository(CoastlineDbContext context) => _context = context;

    public Task<ClassSession?> GetAsync(Guid id, CancellationToken ct = default) =>
        _context.ClassSessions.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<ClassSession>> ListUpcomingAsync(
        DateTime from, CancellationToken ct = default) =>
        await _context.ClassSessions
            .Where(s => s.StartsAt >= from)
            .OrderBy(s => s.StartsAt)
            .ToListAsync(ct);

    public async Task AddAsync(ClassSession session, CancellationToken ct = default) =>
        await _context.ClassSessions.AddAsync(session, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
