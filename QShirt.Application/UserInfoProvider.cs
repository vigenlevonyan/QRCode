using QShirt.Application.Contracts;
using QShirt.Domain;
using System;

namespace QShirt.Application;

public class UserInfoProvider : IUserInfoProvider, IUserInfoSetter
{
    public string UserName { get; set; }

    public string UserTechnicalInfo { get; set; }

    public Guid UserTokenId { get; set; }

    public UserRole UserRole { get; set; }

    public bool IsAuthenticated { get; set; }

    public Guid? AnonymousUserId { get; set; }

    public Guid UserId { get; set; }

    public UserType UserType { get; set; }

    public string OriginalUser { get; set; }
}