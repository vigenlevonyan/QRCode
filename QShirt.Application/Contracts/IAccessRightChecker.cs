using System.Threading.Tasks;

namespace QShirt.Application.Contracts;

/// <summary>
/// Component for checking access rights
/// </summary>
public interface IAccessRightChecker
{
    /// <summary>
    /// Checks that current user is admin
    /// </summary>
    Task CheckIsAdminAsync();

    /// <summary>
    /// Checks that current user is customer
    /// </summary>
    Task CheckIsCustomerAsync();

    /// <summary>
    /// Check is admin or system
    /// </summary>
    Task CheckIsAdminAsyncOrSystem();

    /// <summary>
    /// Checks that current user is anonymous user
    /// </summary>
    void CheckIsAnonymous();

    /// <summary>
    /// Check is system
    /// </summary>
    void CheckIsSystem();
}
