using KSE.Core.Communication;
using KSE.WebApi.Core.User;
using KSE.WebAppMvc.Extensions;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public class AuthService : Services, IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _user;
        private readonly IAuthenticationService _authenticationService;
        public AuthService(HttpClient httpClient, IOptions<AppSettings> settings, IAuthenticationService authenticationService, IAspNetUser user)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _user = user;
        }

        public async Task<ResponseResult> AuthenticatorVerified(TwoFactorAuthenticator twoFactor)
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync("/V1/Authentication/Authenticator", FindContext(twoFactor)));

        public async Task<string[]> GenerateRecovery()
            => await ReturnResponse<string[]>(await _httpClient.GetAsync("/V1/Authentication/GenerateRecovery"));

        public async Task<TwoFactorAuthenticator> GetAuthenticator()
            => await ReturnResponse<TwoFactorAuthenticator>(await _httpClient.GetAsync("/V1/Authentication/Authenticator"));

        public async Task<UserResponseLogin> RefreshToken(string refreshRoken)
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/refresh-token", FindContext(refreshRoken)));

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/FirstAccess", FindContext(userLogin)));

        public async Task<UserResponseLogin> LoginWith2fa(UserLoginWith2fa with2Fa)
        => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/FirstAccess-with2fa", FindContext(with2Fa)));

        public async Task<UserResponseLogin> Register(UserRegister userRegister)
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/Register", FindContext(userRegister)));

        public async Task<ResponseResult> ResetAuthenticator()
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync("/V1/Authentication/ResetAuthenticator", FindContext(null)));


        public static JwtSecurityToken GetTokenFormat(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(_user.FindHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, null);
        }

        public async Task RealizarLogin(UserResponseLogin user)
        {
            var token = GetTokenFormat(user.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", user.AccessToken));
            claims.Add(new Claim("RefreshToken", user.RefreshToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperty = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(_user.FindHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperty);
        }

        public bool TokenExpiration()
        {
            var jwt = _user.GetUserToken();
            if (jwt is null) return false;

            var token = GetTokenFormat(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValid()
        {
            var response = await RefreshToken(_user.GetRefreshToken());

            if(response.AccessToken != null && response.ResponseResult == null)
            {
                await RealizarLogin(response);
                return true;
            }

            return false;
        }
    }
}
