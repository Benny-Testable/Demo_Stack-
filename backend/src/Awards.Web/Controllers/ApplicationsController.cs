using Awards.Web.Data;
using Awards.Web.Models;
using Awards.Web.Models.ViewModels;
using Awards.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Awards.Web.Controllers;

public class ApplicationsController : Controller
{
    private readonly AwardsDbContext _context;
    private readonly LegacyEligibilityScorer _scorer;
    private readonly AwardLetterBuilder _letters;

    public ApplicationsController(
        AwardsDbContext context,
        LegacyEligibilityScorer scorer,
        AwardLetterBuilder letters)
    {
        _context = context;
        _scorer = scorer;
        _letters = letters;
    }

    public async Task<IActionResult> Index(ApplicationStatus? status)
    {
        var query = _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Programme)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        return View(await query.OrderBy(a => a.Reference).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var application = await _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Programme)
            .Include(a => a.Disbursements)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application?.Applicant is null || application.Programme is null)
        {
            return NotFound();
        }

        return View(new ApplicationDetailViewModel
        {
            Application = application,
            ApplicantName = application.Applicant.FullName,
            ProgrammeName = application.Programme.Name,
            RecalculatedScore = _scorer.Score(application.Applicant, application.Programme),
            OutstandingDisbursement = application.Disbursements.Sum(d => d.Outstanding),
            UpcomingDisbursements = application.Disbursements
                .Where(d => !d.IsPaid)
                .OrderBy(d => d.ScheduledFor)
                .ToList()
        });
    }

    [HttpGet("api/applications")]
    public async Task<IActionResult> List()
    {
        var applications = await _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Programme)
            .Select(a => new
            {
                a.Id,
                a.Reference,
                Applicant = a.Applicant!.FullName,
                Programme = a.Programme!.Name,
                a.EligibilityScore,
                Status = a.Status.ToString(),
                a.SubmittedOn,
                a.DecidedOn
            })
            .ToListAsync();

        return Ok(applications);
    }

    [HttpGet("applications/{id}/letter")]
    public async Task<IActionResult> Letter(int id)
    {
        var application = await _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Programme)
            .Include(a => a.Disbursements)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application?.Applicant is null || application.Programme is null)
        {
            return NotFound();
        }

        return Content(_letters.Build(application, application.Applicant, application.Programme),
                       "text/plain");
    }
}
