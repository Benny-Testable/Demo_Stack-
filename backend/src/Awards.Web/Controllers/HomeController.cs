using System.Diagnostics;
using Awards.Web.Data;
using Awards.Web.Models;
using Awards.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Awards.Web.Controllers;

public class HomeController : Controller
{
    private readonly AwardsDbContext _context;

    public HomeController(AwardsDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var programmes = await _context.Programmes.Include(p => p.Applications).ToListAsync();
        var applications = programmes.SelectMany(p => p.Applications).ToList();

        var model = new AwardsSummaryViewModel
        {
            ProgrammeCount = programmes.Count,
            ApplicationCount = applications.Count,
            AwardedCount = applications.Count(a => a.Status == ApplicationStatus.Awarded),
            TotalAwarded = programmes.Sum(p =>
                p.Applications.Count(a => a.Status == ApplicationStatus.Awarded) * p.AwardAmount),
            Programmes = programmes.Select(p => new AwardsSummaryViewModel.ProgrammeRow
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Category = p.Category.ToString(),
                Applications = p.Applications.Count,
                PlacesAvailable = p.PlacesAvailable,
                AwardAmount = p.AwardAmount
            }).ToList()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}
