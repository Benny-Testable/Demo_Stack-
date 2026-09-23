using System.Text;
using Awards.Web.Models;

namespace Awards.Web.Services;

public class AwardLetterBuilder
{
    public string Build(Application application, Applicant applicant, Programme programme)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SCHOLARSHIP AWARD LETTER");
        builder.AppendLine(new string('=', 54));
        builder.AppendLine($"Reference   : {application.Reference}");
        builder.AppendLine($"Applicant   : {applicant.FullName}");
        builder.AppendLine($"Email       : {applicant.Email}");
        builder.AppendLine($"Institution : {applicant.Institution}");
        builder.AppendLine($"Programme   : {programme.Name} ({programme.Code})");
        builder.AppendLine(new string('-', 54));
        builder.AppendLine($"Submitted   : {application.SubmittedOn:dd MMM yyyy}");
        builder.AppendLine($"Score       : {application.EligibilityScore:N2}");
        builder.AppendLine($"Status      : {application.Status}");
        builder.AppendLine($"Award value : {programme.AwardAmount:N2}");

        decimal outstanding = 0m;
        foreach (var disbursement in application.Disbursements)
        {
            outstanding += disbursement.Outstanding;
            builder.AppendLine(
                $"  {disbursement.ScheduledFor:dd MMM yyyy}  due {disbursement.Amount,11:N2}  paid {disbursement.AmountPaid,11:N2}");
        }

        builder.AppendLine(new string('-', 54));
        builder.AppendLine($"Outstanding : {outstanding:N2}");
        builder.AppendLine($"Congratulations, your application has been successful.");
        builder.AppendLine(new string('=', 54));

        return builder.ToString();
    }
}
