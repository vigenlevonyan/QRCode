using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.IdentityModel.Tokens;
using QShirt.Application;
using QShirt.Application.AuthOptions;
using QShirt.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QShirt.Public.Server;

public class UserInfoSetterInterceptor : Interceptor
{
    private readonly UserInfoProvider userInfoProvider;

    public UserInfoSetterInterceptor(UserInfoProvider userInfoProvider)
    {
        this.userInfoProvider = userInfoProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var authorizationHeader =
            context.RequestHeaders.FirstOrDefault(h =>
                string.Equals(h.Key, "Authorization", StringComparison.OrdinalIgnoreCase));
        if (authorizationHeader == null)
            return await continuation(request, context);

        var token = authorizationHeader.Value.Substring(7);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, 
            ValidIssuer = AdminAuthOptions.Issuer, 

            ValidateAudience = true, 
            ValidAudience = AdminAuthOptions.Audience,

            ValidateLifetime = false, 

            IssuerSigningKey = AdminAuthOptions.GetSymmetricSecurityKey(), 
            ValidateIssuerSigningKey = true 
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        var user = handler.ValidateToken(token, tokenValidationParameters, out _);

        var userName = user.Identity?.Name;
        string idFromTokenStr = user.Claims
            .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userName != null)
        {
            userInfoProvider.UserName = userName;
            if (idFromTokenStr != null)
                userInfoProvider.UserTokenId = Guid.Parse(idFromTokenStr);

            userInfoProvider.IsAuthenticated = true;
            if (user.IsInRole(UserRole.Admin.ToString()))
                userInfoProvider.UserRole = UserRole.Admin;
        }
        else
            userInfoProvider.IsAuthenticated = false;

        return await continuation(request, context);
    }
}