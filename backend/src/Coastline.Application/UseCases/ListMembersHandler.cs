using Coastline.Application.Abstractions;
using Coastline.Application.Dtos;

namespace Coastline.Application.UseCases;

public class ListMembersHandler
{
    private readonly IMemberRepository _members;

    public ListMembersHandler(IMemberRepository members) => _members = members;

    public async Task<IReadOnlyList<MemberDto>> HandleAsync(CancellationToken ct = default)
    {
        var members = await _members.ListAsync(ct);

        return members
            .OrderBy(m => m.FullName)
            .Select(m => new MemberDto(m.Id, m.FullName, m.Email, m.Tier.ToString(),
                                       m.JoinedOn, m.IsSuspended, m.Bookings.Count))
            .ToList();
    }
}
