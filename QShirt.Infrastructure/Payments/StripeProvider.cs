using AutoMapper;
using QShirt.Application.Contracts.Infrastructure.Payments;
using QShirt.Application.Contracts.Infrastructure.Payments.Models;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QShirt.Infrastructure.Payments;

public class StripeProvider : IStripeProvider
{
    #region Fields

    private readonly string secretKey;
    private readonly string domain;
    private readonly IMapper mapper;

    #endregion Fields

    #region Constructor

    public StripeProvider(string secretKey, string domain, IMapper mapper)
    {
        StripeConfiguration.ApiKey = secretKey;
        this.secretKey = secretKey;
        this.domain = domain;
        this.mapper = mapper;
    }

    #endregion Constructor

    #region Methods

    public async Task<SessionModel> CreateCheckoutSession(CreateSessionModel model)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = model.Items.Select(item => new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = model.Currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.ProductName,
                        Images = new List<string> { item.ImageUrl }
                    },
                    UnitAmountDecimal = item.Price * 100
                },
                Quantity = item.Quantity
            }).ToList(),
            Mode = "payment",
            SuccessUrl = domain + model.SuccessUrl,
            CancelUrl = domain + model.CancelUrl,
            ConsentCollection = new Stripe.Checkout.SessionConsentCollectionOptions
            {
                TermsOfService = "required",
            },
            CustomText = new SessionCustomTextOptions
            {
                TermsOfServiceAcceptance = new SessionCustomTextTermsOfServiceAcceptanceOptions
                {
                    Message = $"I agree to the [Terms of Service]({domain}privacy-policy)",
                }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return new SessionModel
        {
            SessionId = session.Id,
            Url = session.Url
        };
    }

    public async Task<SessionResultModel> GetSession(string sessionId)
    {
        var sessionService = new SessionService();
        Session session = sessionService.Get(sessionId);
        var sessionModel = mapper.Map<SessionResultModel>(session);
        return sessionModel;
    }

    #endregion Methods
}
