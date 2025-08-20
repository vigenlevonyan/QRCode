namespace QShirt.Application.Contracts.Infrastructure.Payments.Models;

/// <summary>
/// Created session model
/// </summary>
public class SessionModel
{
    /// <summary>
    /// Created session ID
    /// </summary>
    public string SessionId { get; set; }

    /// <summary>
    /// Url for redirect, after create session
    /// </summary>
    public string Url { get; set; }
}
