using System.Text;
using Estates.Web.Models;

namespace Estates.Web.Services;

public class InvoiceReportBuilder
{
    public string Build(Lease lease, Property property, Tenant tenant)
    {
        var builder = new StringBuilder();

        builder.AppendLine("RENT INVOICE STATEMENT");
        builder.AppendLine(new string('=', 48));
        builder.AppendLine($"Reference   : {lease.Reference}");
        builder.AppendLine($"Property    : {property.Name} (Block {property.BlockNumber})");
        builder.AppendLine($"Address     : {property.AddressLine}, {property.City} {property.PostCode}");
        builder.AppendLine($"Tenant      : {tenant.CompanyName}");
        builder.AppendLine($"Contact     : {tenant.ContactEmail}");
        builder.AppendLine(new string('-', 48));
        builder.AppendLine($"Term        : {lease.StartDate:dd MMM yyyy} to {lease.EndDate:dd MMM yyyy}");
        builder.AppendLine($"Months      : {lease.TermMonths}");
        builder.AppendLine($"Units       : {lease.UnitCount}");
        builder.AppendLine($"Base rent   : {lease.BaseAnnualRent:N2}");
        builder.AppendLine($"Service     : {lease.ServiceCharge:N2}");
        builder.AppendLine($"Deposit     : {lease.DepositHeld:N2}");
        builder.AppendLine(new string('-', 48));

        decimal outstanding = 0m;
        foreach (var payment in lease.Payments)
        {
            outstanding += payment.Outstanding;
            builder.AppendLine(
                $"  {payment.DueOn:dd MMM yyyy}  due {payment.AmountDue,12:N2}  paid {payment.AmountPaid,12:N2}");
        }

        builder.AppendLine(new string('-', 48));
        builder.AppendLine($"Outstanding : {outstanding:N2}");
        builder.AppendLine($"Status      : {lease.Status}");
        var renewal = lease.IsRenewal ? "yes" : "no";
        builder.AppendLine($"Renewal     : {renewal}");
        builder.AppendLine(new string('=', 48));

        return builder.ToString();
    }
}
