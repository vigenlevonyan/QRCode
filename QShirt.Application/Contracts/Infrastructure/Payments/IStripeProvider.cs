using QShirt.Application.Contracts.Infrastructure.Payments.Models;
using System.Threading.Tasks;

namespace QShirt.Application.Contracts.Infrastructure.Payments;

public interface IStripeProvider
{
    public Task<SessionModel> CreateCheckoutSession(CreateSessionModel model);

    public Task<SessionResultModel> GetSession(string sessionId);

}
