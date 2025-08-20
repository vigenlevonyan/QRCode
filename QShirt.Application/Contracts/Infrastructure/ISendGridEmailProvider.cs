using System.Threading.Tasks;

namespace QShirt.Application.Contracts.Infrastructure;

public interface ISendGridEmailProvider
{
    Task<bool> SendEmail(string toEmail, string subject, string body, string htmlContent);
}
