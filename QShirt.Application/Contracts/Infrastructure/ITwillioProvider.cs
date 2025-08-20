using System.Threading.Tasks;

namespace QShirt.Application.Contracts.Infrastructure;

public interface ITwillioProvider
{
    Task SendSms(string to, string text);
}
