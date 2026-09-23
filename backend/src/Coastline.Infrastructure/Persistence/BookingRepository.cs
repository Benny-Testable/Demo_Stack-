using Coastline.Application.Abstractions;
using Coastline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coastline.Infrastructure.Persistence;

public class BookingRepository : IBookingRepository
{
    private readonly CoastlineDbContext _context;

    public BookingRepository(CoastlineDbContext context) => _context = context;

    public Task<Booking?> GetAsync(Guid id, CancellationToken ct = default) =>
        _context.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IReadOnlyList<Booking>> ListForMemberAsync(
        Guid memberId, CancellationToken ct = default) =>
        await _context.Bookings.Where(b => b.MemberId == memberId).ToListAsync(ct);

    public async Task AddAsync(Booking booking, CancellationToken ct = default) =>
        await _context.Bookings.AddAsync(booking, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
