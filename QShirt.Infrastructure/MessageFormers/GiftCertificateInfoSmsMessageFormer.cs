using QShirt.Application.Contracts.Infrastructure.MessageFormers;
using QShirt.Application.Contracts.Infrastructure.Models;
using System.Collections.Generic;

namespace QShirt.Infrastructure.MessageFormers;

public class GiftCertificateInfoSmsMessageFormer : IGiftCertificateInfoSmsMessageFormer
{
    #region Methods

    /// <summary>
    /// SMS message text formatter for gift certificate receipt
    /// </summary>
    public string Form(IEnumerable<SmsMessageModel> orderCertificates)
    {
        string text = string.Empty;

        //foreach (var orderCertificate in orderCertificates)
        //{
        //    text = $"{text} {orderCertificate.ProductName} номинал {orderCertificate.Nominal} {orderCertificate.Unit} сертификат №{orderCertificate.Number} PIN {orderCertificate.Pin}";
        //    text = orderCertificate.ExpirationDateTime.HasValue ? $"{te
        //    xt} действителен до {orderCertificate.ExpirationDateTime.Value.ToShortDateString()} %0A" : $"{text}";
        //}
        return text;
    }

    #endregion Methods
}
