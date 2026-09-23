using Awards.Web.Data;
using Awards.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Awards.Web.Controllers;

public class ApplicantsController : Controller
{
    private readonly AwardsDbContext _context;

    public ApplicantsController(AwardsDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Applicants.Include(a => a.Applications).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var safe = ApplicantSanitizer.Clean(search);
            query = query.Where(a => a.FullName.Contains(safe) || a.Institution.Contains(safe));
        }

        return View(await query.OrderBy(a => a.FullName).ToListAsync());
    }

    [HttpGet("api/applicants")]
    public async Task<IActionResult> List()
    {
        var applicants = await _context.Applicants
            .Select(a => new
            {
                a.Id,
                a.FullName,
                a.Email,
                a.Institution,
                a.YearOfStudy,
                a.GradePointAverage,
                a.IsFirstGeneration,
                ApplicationCount = a.Applications.Count
            })
            .ToListAsync();

        return Ok(applicants);
    }
}
