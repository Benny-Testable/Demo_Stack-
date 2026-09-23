using System.Diagnostics;
using Estates.Web.Data;
using Estates.Web.Models;
using Estates.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estates.Web.Controllers;

public class HomeController : Controller
{
    private readonly EstatesDbContext _context;

    public HomeController(EstatesDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var properties = await _context.Properties
            .Include(p => p.Leases)
            .ToListAsync();

        var model = new PortfolioSummaryViewModel
        {
            PropertyCount = properties.Count,
            ActiveLeaseCount = properties.SelectMany(p => p.Leases)
                                         .Count(l => l.Status == LeaseStatus.Active),
            TotalAnnualRent = properties.SelectMany(p => p.Leases).Sum(l => l.BaseAnnualRent),
            Properties = properties.Select(p => new PortfolioSummaryViewModel.PropertyRow
            {
                Id = p.Id,
                Name = p.Name,
                City = p.City,
                BlockNumber = p.BlockNumber,
                LeaseCount = p.Leases.Count,
                AnnualRent = p.Leases.Sum(l => l.BaseAnnualRent),
                OccupancyPct = p.TotalUnits == 0
                    ? 0m
                    : Math.Round(p.Leases.Sum(l => l.UnitCount) * 100m / p.TotalUnits, 1)
            }).ToList()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
