using Coastline.Application.Abstractions;
using Coastline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coastline.Infrastructure.Persistence;

public class MemberRepository : IMemberRepository
{
    private readonly CoastlineDbContext _context;

    public MemberRepository(CoastlineDbContext context) => _context = context;

    public Task<Member?> GetAsync(Guid id, CancellationToken ct = default) =>
        _context.Members.Include(m => m.Bookings).FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<IReadOnlyList<Member>> ListAsync(CancellationToken ct = default) =>
        await _context.Members.Include(m => m.Bookings).ToListAsync(ct);

    public async Task AddAsync(Member member, CancellationToken ct = default) =>
        await _context.Members.AddAsync(member, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
