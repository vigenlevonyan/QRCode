using QShirt.Application.Contracts.Infrastructure;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace QShirt.Infrastructure.SmsGate;

public class TwillioProvider : ITwillioProvider
{
    #region Fields

    public string Sid { get; }
    public string Token { get; }
    public string Sender { get; }
    public string MessagingServiceSid { get; }

    #endregion fields

    #region Constructor
    public TwillioProvider(string sid, string token, string sender, string messagingServiceSid)
    {
        Sid = sid;
        Token = token;
        Sender = sender;
        MessagingServiceSid = messagingServiceSid;
    }


    #endregion Constructor

    #region Methods

    public async Task SendSms (string to, string text)
    {
        TwilioClient.Init(Sid, Token);

        var messageOptions = new CreateMessageOptions(
          new PhoneNumber($"+{to}"));
        //messageOptions.From = new PhoneNumber(Sender);
        messageOptions.Body = text;
        messageOptions.MessagingServiceSid = MessagingServiceSid;


        var message = MessageResource.Create(messageOptions);
    }

    #endregion Methods
}
