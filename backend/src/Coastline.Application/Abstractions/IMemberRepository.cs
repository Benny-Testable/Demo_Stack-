using Coastline.Domain.Entities;

namespace Coastline.Application.Abstractions;

public interface IMemberRepository
{
    Task<Member?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Member>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Member member, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
