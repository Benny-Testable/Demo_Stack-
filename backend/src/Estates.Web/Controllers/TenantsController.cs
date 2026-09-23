using Estates.Web.Data;
using Estates.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estates.Web.Controllers;

public class TenantsController : Controller
{
    private readonly EstatesDbContext _context;

    public TenantsController(EstatesDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Tenants.Include(t => t.Leases).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var safe = InputSanitizer.StripUnsafe(search);
            query = query.Where(t => t.CompanyName.Contains(safe));
        }

        return View(await query.OrderBy(t => t.CompanyName).ToListAsync());
    }

    [HttpGet("api/tenants")]
    public async Task<IActionResult> List()
    {
        var tenants = await _context.Tenants
            .Select(t => new
            {
                t.Id,
                t.CompanyName,
                t.ContactEmail,
                t.CreditScore,
                t.IsAnchorTenant,
                LeaseCount = t.Leases.Count
            })
            .ToListAsync();

        return Ok(tenants);
    }
}
