using QShirt.Domain;
using System;

namespace QShirt.Application.Contracts;

/// <summary>
/// Setter for current user information
/// </summary>
public interface IUserInfoSetter : IUserInfoProvider
{
    /// <summary>User name</summary>
    new string UserName { get; set; }

    /// <summary>User technical information</summary>
    new string UserTechnicalInfo { get; set; }

    /// <summary>User type</summary>
    new UserType UserType { get; set; }

    /// <summary>User type</summary>
    new bool IsAuthenticated { get; set; }

    /// <summary>User authorization token ID</summary>
    new Guid UserTokenId { get; set; }

    /// <summary>
    /// User role
    /// </summary>
    new UserRole UserRole { get; set; }

    /// <summary>
    /// Original user
    /// </summary>
    new string OriginalUser { get; set; }
}
