using Coastline.Domain.Entities;

namespace Coastline.Application.Abstractions;
## new commit 
public interface IBookingRepository
{
    Task<Booking?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> ListForMemberAsync(Guid memberId, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
