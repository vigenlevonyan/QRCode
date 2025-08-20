using QShirt.Domain;
using System;

namespace QShirt.Application.Contracts;

/// <summary>
/// Provider of system user data
/// </summary>
public interface IUserInfoProvider
{
    /// <summary>
    /// User name
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// User technical information
    /// </summary>
    string UserTechnicalInfo { get; }

    /// <summary>
    /// User role
    /// </summary>
    UserRole UserRole { get; }

    /// <summary>
    /// User authorization token ID
    /// </summary>
    Guid UserTokenId { get; }

    /// <summary>
    /// Is authenticated?
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Anonymous user ID
    /// </summary>
    Guid? AnonymousUserId { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    /// User type
    /// </summary>
    UserType UserType { get; set; }

    /// <summary>
    /// Original user
    /// </summary>
    string OriginalUser { get; set; }
}
