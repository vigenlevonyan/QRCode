using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace QShirt.Application.AuthOptions;

/// <summary>
/// JWT token authorization options for administrator
/// </summary>
public class AdminAuthOptions
{
    /// <summary>
    /// Token issuer
    /// </summary>
    public const string Issuer = "App_Admin";

    /// <summary>
    /// Token audience
    /// </summary>
    public const string Audience = "App_Admin";

    /// <summary>
    /// Encryption key
    /// </summary>
    private const string Key = "App_dd4d*038(4$5jkanhsdkjds;kksfgg{fvdov+7f9";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
