namespace QShirt.Application.Contracts.Infrastructure.Payments.Models;

public class ResponseArcaSessionModel
{
    /// <summary>
    /// Uniqe order Id
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// For customer redirect
    /// </summary>
    public string FormUrl { get; set; }

    /// <summary>
    /// Error code
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Error code to string
    /// </summary>
    public string ErrorCodeString { get; set; }

    /// <summary>
    /// Error description
    /// </summary>
    public string ErrorMessage { get; set; }
}
