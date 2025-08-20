using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace QShirt.Application.AuthOptions;

public class PublicAuthOptions
{
    /// <summary>
    /// Token issuer
    /// </summary>
    public const string Issuer = "App_Public";

    /// <summary>
    /// Token audience
    /// </summary>
    public const string Audience = "App_Public";

    /// <summary>
    /// Encryption key
    /// </summary>
    private const string Key = "QShirt_65sd4f3sdf!!@#DF@#$F@#$F@$F@$F";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
