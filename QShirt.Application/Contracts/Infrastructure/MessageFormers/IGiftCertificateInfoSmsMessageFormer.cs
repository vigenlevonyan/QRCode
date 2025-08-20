using QShirt.Application.Contracts.Infrastructure.Models;
using System.Collections.Generic;

namespace QShirt.Application.Contracts.Infrastructure.MessageFormers
{
    /// <summary>
    /// SMS message text formatter for gift certificate receipt
    /// </summary>
    public interface IGiftCertificateInfoSmsMessageFormer
    {
        /// <summary>
        /// SMS message text formatter for gift certificate receipt
        /// </summary>
        string Form(IEnumerable<SmsMessageModel> orderCertificates);
    }
}
