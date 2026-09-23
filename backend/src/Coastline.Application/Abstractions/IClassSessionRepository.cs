using Coastline.Domain.Entities;

namespace Coastline.Application.Abstractions;

public interface IClassSessionRepository
{
    Task<ClassSession?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ClassSession>> ListUpcomingAsync(DateTime from, CancellationToken ct = default);
    Task AddAsync(ClassSession session, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
