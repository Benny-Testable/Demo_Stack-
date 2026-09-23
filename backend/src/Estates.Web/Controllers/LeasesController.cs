using Estates.Web.Data;
using Estates.Web.Models;
using Estates.Web.Models.ViewModels;
using Estates.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estates.Web.Controllers;

public class LeasesController : Controller
{
    private readonly EstatesDbContext _context;
    private readonly LegacyRentPricingService _pricing;
    private readonly LeaseReportBuilder _reports;

    public LeasesController(
        EstatesDbContext context,
        LegacyRentPricingService pricing,
        LeaseReportBuilder reports)
    {
        _context = context;
        _pricing = pricing;
        _reports = reports;
    }

    public async Task<IActionResult> Index(LeaseStatus? status)
    {
        var query = _context.Leases
            .Include(l => l.Property)
            .Include(l => l.Tenant)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }

        return View(await query.OrderBy(l => l.Reference).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var lease = await _context.Leases
            .Include(l => l.Property)
            .Include(l => l.Tenant)
            .Include(l => l.Payments)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lease?.Property is null || lease.Tenant is null)
        {
            return NotFound();
        }

        return View(new LeaseDetailViewModel
        {
            Lease = lease,
            PropertyName = lease.Property.Name,
            TenantName = lease.Tenant.CompanyName,
            CalculatedRent = _pricing.CalculateAnnualRent(lease, lease.Property, lease.Tenant),
            OutstandingBalance = lease.Payments.Sum(p => p.Outstanding),
            UpcomingPayments = lease.Payments
                .Where(p => !p.IsSettled)
                .OrderBy(p => p.DueOn)
                .ToList()
        });
    }

    [HttpGet("api/leases")]
    public async Task<IActionResult> List()
    {
        var leases = await _context.Leases
            .Include(l => l.Property)
            .Include(l => l.Tenant)
            .Select(l => new
            {
                l.Id,
                l.Reference,
                Property = l.Property!.Name,
                Tenant = l.Tenant!.CompanyName,
                l.BaseAnnualRent,
                l.UnitCount,
                Status = l.Status.ToString(),
                l.StartDate,
                l.EndDate
            })
            .ToListAsync();

        return Ok(leases);
    }

    [HttpGet("leases/{id}/report")]
    public async Task<IActionResult> Report(int id)
    {
        var lease = await _context.Leases
            .Include(l => l.Property)
            .Include(l => l.Tenant)
            .Include(l => l.Payments)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lease?.Property is null || lease.Tenant is null)
        {
            return NotFound();
        }

        return Content(_reports.Build(lease, lease.Property, lease.Tenant), "text/plain");
    }
}
