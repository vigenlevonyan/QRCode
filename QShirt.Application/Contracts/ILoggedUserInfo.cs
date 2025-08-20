namespace QShirt.Application.Contracts;

public interface ILoggedUserInfo
{
    string UserName { get; }
    string UserTechnicalInfo { get; }
    string OriginalUser { get; }
}
