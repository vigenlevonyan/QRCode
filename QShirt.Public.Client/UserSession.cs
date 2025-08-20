using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using QShirt.Public.Client.InteropServices;
using System.Security.Claims;

namespace QShirt.Public.Client
{
    /// <summary>
    /// User session
    /// </summary>
    public class UserSession : AuthenticationStateProvider
    {
        #region Consts

        private const string NameKey = "nc.admin.name";
        private const string TokenKey = "nc.admin.token";
        private const string SessionStartedKey = "nc.admin.issessionstarted";

        #endregion Consts

        #region Fields

        private readonly ILocalStorageService localStorage;
        private readonly Cookies cookies;

        // private readonly IAppUserInfoSetter appUserInfoSetter;

        #endregion Fields

        #region Constructor

        public UserSession(ILocalStorageService localStorage, Cookies cookies)
        {
            this.localStorage = localStorage;
            this.cookies = cookies;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Is session started?
        /// </summary>
        public bool IsSessionStarted { get; private set; }

        /// <summary>
        /// Is session loaded
        /// </summary>
        public bool IsSessionLoaded { get; set; }

        #endregion Properties

        #region Methods

        #region Private methods

        private void SetUserInfo()
        {
            // if (Token != null)
            // {
            //     var tokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuer = true, // indicates whether the issuer will be validated during token validation
            //         ValidIssuer = AdminAuthOptions.Issuer, // string representing the issuer
            //
            //         ValidateAudience = false, // whether the token audience will be validated
            //         ValidAudience = AdminAuthOptions.Audience,// setting the token audience
            //
            //         ValidateLifetime = false,  // whether the lifetime will be validated
            //
            //         IssuerSigningKey = AdminAuthOptions.GetSymmetricSecurityKey(), // setting the security key
            //         ValidateIssuerSigningKey = true // security key validation
            //     };
            //
            //     JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            //     var user = handler.ValidateToken(Token, tokenValidationParameters, out SecurityToken validatedToken);
            //
            //     appUserInfoSetter.UserName = user.Identity.Name;
            //     appUserInfoSetter.UserType = UserType.Authenticated;
            //     appUserInfoSetter.UserRole = UserRole.Admin;
            //     string userTokenIdStr = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //     if (!string.IsNullOrWhiteSpace(userTokenIdStr))
            //         appUserInfoSetter.UserTokenId = Guid.Parse(userTokenIdStr);
            // }
            // else
            // {
            //     appUserInfoSetter.UserName = null;
            //     appUserInfoSetter.UserType = UserType.Anonymous;
            //     appUserInfoSetter.UserTokenId = Guid.Empty;
            // }
        }

        #endregion Private methods

        #region Public methods

        /// <summary>
        /// Loads user session
        /// </summary>
        public async Task LoadSession()
        {
            Name = await localStorage.GetItemAsync<string>(NameKey);
            Token = await localStorage.GetItemAsync<string>(TokenKey);
            IsSessionStarted = await localStorage.GetItemAsync<bool>(SessionStartedKey);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            IsSessionLoaded = true;

            await cookies.SetAsync(TokenKey, Token, 365);

            SetUserInfo();
        }

        /// <summary>
        /// Starts user session
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="token">user token</param>
        public async Task StartSession(string name, string token)
        {
            await localStorage.SetItemAsync(NameKey, name);
            await localStorage.SetItemAsync(TokenKey, token);
            await localStorage.SetItemAsync(SessionStartedKey, true);

            Name = name;
            Token = token;
            IsSessionStarted = true;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            await cookies.SetAsync(TokenKey, Token, 365);

            SetUserInfo();
        }

        /// <summary>
        /// Ends user session
        /// </summary>
        public async Task FinishSession()
        {
            await localStorage.RemoveItemAsync(NameKey);
            await localStorage.RemoveItemAsync(TokenKey);
            await localStorage.SetItemAsync(SessionStartedKey, false);

            Name = null;
            Token = null;
            IsSessionStarted = false;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            await cookies.RemoveAsync(TokenKey);

            SetUserInfo();
        }

        #endregion Public methods

        #endregion Methods

        #region AuthenticationStateProvider

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!IsSessionLoaded)
                await LoadSession();

            if (!IsSessionStarted)
            {
                ClaimsIdentity anonymous = new ClaimsIdentity();
                return new AuthenticationState(new ClaimsPrincipal(anonymous));
            }

            ClaimsIdentity identity = new ClaimsIdentity("Authorize");
            identity.AddClaim(new Claim(ClaimTypes.Name, Name));
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        #endregion AuthenticationStateProvider
    }
}