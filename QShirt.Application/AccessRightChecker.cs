using Microsoft.EntityFrameworkCore;
using QShirt.Application.Contracts;
using QShirt.Application.Exceptions;
using QShirt.Domain;
using QShirt.Persistence;
using System.Threading.Tasks;

namespace QShirt.Application;

public class AccessRightChecker : IAccessRightChecker
{
    #region Consts

    private const string AccessDeniedErrorMessage = "Access denied";

    #endregion Consts

    #region Fields

    private readonly IRepository<Administrator> administratorRepository;
    private readonly IRepository<Customer> customersRepository;
    private readonly IUserInfoProvider userInfoProvider;

    #endregion Fields

    #region Constructor

    public AccessRightChecker(
        IRepository<Administrator> administratorRepository,
        IRepository<Customer> customersRepository,
        IUserInfoProvider userInfoProvider
        )
    {
        this.administratorRepository = administratorRepository;
        this.customersRepository = customersRepository;
        this.userInfoProvider = userInfoProvider;
    }

    #endregion Constructor

    #region Methods

    #region Private methods

    private bool IsUserInRole(UserRole userRole)
    {
        return userInfoProvider.UserType == UserType.Authenticated &&
               userInfoProvider.UserRole == userRole;
    }

    #endregion Private methods

    #region Public methods

    public async Task CheckIsAdminAsync()
    {
        bool administratorWithActualToken = await administratorRepository
            .AnyAsync(a => a.AuthTokenId == userInfoProvider.UserTokenId);
        if (!administratorWithActualToken)
            throw new AccessDeniedException("Access Denied");

        if (!IsUserInRole(UserRole.Admin))
            throw new AccessDeniedException(AccessDeniedErrorMessage);
    }

    public void CheckIsSystem()
    {
        if (userInfoProvider.UserType != UserType.System)
            throw new AccessDeniedException("Access Denied");
    }

    public async Task CheckIsAdminAsyncOrSystem()
    {
        bool administratorWithActualToken = await administratorRepository
            .AnyAsync(a => a.AuthTokenId == userInfoProvider.UserTokenId);

        if (userInfoProvider.UserType != UserType.System && !administratorWithActualToken && !IsUserInRole(UserRole.Admin))
            throw new AccessDeniedException("Access Denied");
    }

    public async Task CheckIsCustomerAsync()
    {
        bool customerWithActualToken = await customersRepository
            .AnyAsync(c => c.AuthTokenId == userInfoProvider.UserTokenId);
        if (!customerWithActualToken)
            throw new AccessDeniedException("Access Denied");

        if (!IsUserInRole(UserRole.Customer))
            throw new AccessDeniedException(AccessDeniedErrorMessage);
    }

    public void CheckIsAnonymous()
    {
        if (userInfoProvider.IsAuthenticated)
            throw new AccessDeniedException(AccessDeniedErrorMessage);
    }

    #endregion Public methods

    #endregion Methods
}
