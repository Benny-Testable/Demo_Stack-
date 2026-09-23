using Coastline.Application.Abstractions;
using Coastline.Application.Dtos;
using Coastline.Application.Services;
using Coastline.Application.UseCases;
using Coastline.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Coastline.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly ListMembersHandler _listMembers;
    private readonly IMemberRepository _members;

    public MembersController(ListMembersHandler listMembers, IMemberRepository members)
    {
        _listMembers = listMembers;
        _members = members;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MemberDto>>> List(CancellationToken ct) =>
        Ok(await _listMembers.HandleAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberDto>> Get(Guid id, CancellationToken ct)
    {
        var member = await _members.GetAsync(id, ct);
        if (member is null)
        {
            return NotFound();
        }

        return Ok(new MemberDto(member.Id, member.FullName, member.Email,
                                member.Tier.ToString(), member.JoinedOn,
                                member.IsSuspended, member.Bookings.Count));
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create(CreateMemberRequest request, CancellationToken ct)
    {
        if (!MemberInputSanitizer.IsValidEmail(request.Email))
        {
            return BadRequest("A valid email address is required");
        }
        if (!Enum.TryParse<MembershipTier>(request.Tier, out var tier))
        {
            return BadRequest($"Unknown membership tier '{request.Tier}'");
        }

        var member = new Member(
            MemberInputSanitizer.NormaliseName(request.FullName),
            request.Email,
            tier,
            DateOnly.FromDateTime(DateTime.UtcNow));

        await _members.AddAsync(member, ct);
        await _members.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(Get), new { id = member.Id },
            new MemberDto(member.Id, member.FullName, member.Email, member.Tier.ToString(),
                          member.JoinedOn, member.IsSuspended, 0));
    }
}
