using QShirt.Application.Contracts.Infrastructure;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace QShirt.Infrastructure.EmailProvider;

public class SendGridEmailProvider : ISendGridEmailProvider
{
    private readonly string apiKey;
    private readonly string sender;
    private readonly string company;

    public SendGridEmailProvider(string apiKey, string sender, string company)
    {
        this.apiKey = apiKey;
        this.sender = sender;
        this.company = company;
    }

    public async Task<bool> SendEmail(string toEmail, string subject, string body, string htmlContent)
    {
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(sender, company);
        var to = new EmailAddress(toEmail);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, htmlContent);
        Response response = await client.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }
}
