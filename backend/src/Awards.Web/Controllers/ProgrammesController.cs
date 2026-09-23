using Awards.Web.Data;
using Awards.Web.Models;
using Awards.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Awards.Web.Controllers;

public class ProgrammesController : Controller
{
    private readonly AwardsDbContext _context;

    public ProgrammesController(AwardsDbContext context) => _context = context;

    public async Task<IActionResult> Index(ProgrammeCategory? category)
    {
        var query = _context.Programmes.Include(p => p.Applications).AsQueryable();

        if (category.HasValue)
        {
            query = query.Where(p => p.Category == category.Value);
        }

        return View(await query.OrderBy(p => p.Code).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var programme = await _context.Programmes
            .Include(p => p.Applications).ThenInclude(a => a.Applicant)
            .FirstOrDefaultAsync(p => p.Id == id);

        return programme is null ? NotFound() : View(programme);
    }

    [HttpGet("api/programmes")]
    public async Task<IActionResult> List()
    {
        var programmes = await _context.Programmes
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Code,
                Category = p.Category.ToString(),
                p.AwardAmount,
                p.PlacesAvailable,
                p.IsOpen,
                ApplicationCount = p.Applications.Count
            })
            .ToListAsync();

        return Ok(programmes);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Programme programme)
    {
        if (!ModelState.IsValid)
        {
            return View(programme);
        }

        programme.Code = ApplicantSanitizer.Clean(programme.Code).ToUpperInvariant();
        _context.Programmes.Add(programme);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
