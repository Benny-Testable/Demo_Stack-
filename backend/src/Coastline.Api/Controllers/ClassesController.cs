using Coastline.Application.Abstractions;
using Coastline.Application.Dtos;
using Coastline.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Coastline.Api.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly IClassSessionRepository _sessions;
    private readonly BookClassHandler _bookClass;

    public ClassesController(IClassSessionRepository sessions, BookClassHandler bookClass)
    {
        _sessions = sessions;
        _bookClass = bookClass;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClassSessionDto>>> Upcoming(CancellationToken ct)
    {
        var sessions = await _sessions.ListUpcomingAsync(DateTime.UtcNow, ct);

        return Ok(sessions.Select(s => new ClassSessionDto(
            s.Id, s.Title, s.Instructor, s.StartsAt, s.DurationMinutes,
            s.Capacity, s.RemainingPlaces, s.MinimumTier.ToString())).ToList());
    }

    [HttpPost("{sessionId:guid}/bookings")]
    public async Task<ActionResult<BookingDto>> Book(Guid sessionId, [FromQuery] Guid memberId,
                                                     CancellationToken ct)
    {
        try
        {
            return Ok(await _bookClass.HandleAsync(memberId, sessionId, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
