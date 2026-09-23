using Estates.Web.Data;
using Estates.Web.Models;
using Estates.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estates.Web.Controllers;

public class PropertiesController : Controller
{
    private readonly EstatesDbContext _context;

    public PropertiesController(EstatesDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? city)
    {
        var query = _context.Properties.Include(p => p.Leases).AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
        {
            var safeCity = InputSanitizer.StripUnsafe(city);
            query = query.Where(p => p.City == safeCity);
        }

        return View(await query.OrderBy(p => p.BlockNumber).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var property = await _context.Properties
            .Include(p => p.Leases).ThenInclude(l => l.Tenant)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property is null)
        {
            return NotFound();
        }

        return View(property);
    }

    [HttpGet("api/properties")]
    public async Task<IActionResult> List()
    {
        var properties = await _context.Properties
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.City,
                p.BlockNumber,
                p.TotalUnits,
                Grade = p.Grade.ToString(),
                LeaseCount = p.Leases.Count
            })
            .ToListAsync();

        return Ok(properties);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Property property)
    {
        if (!ModelState.IsValid)
        {
            return View(property);
        }

        property.PostCode = InputSanitizer.NormalisePostCode(property.PostCode);
        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
