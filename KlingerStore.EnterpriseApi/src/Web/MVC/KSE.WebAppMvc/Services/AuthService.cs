using KSE.Core.Communication;
using KSE.WebAppMvc.Extensions;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public class AuthService : Services, IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);
            _httpClient = httpClient;
        }

        public async Task<ResponseResult> AuthenticatorVerified(TwoFactorAuthenticator twoFactor)
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync("/V1/Authentication/Authenticator", FindContext(twoFactor)));

        public async Task<TwoFactorAuthenticator> GetAuthenticator()
            => await ReturnResponse<TwoFactorAuthenticator>(await _httpClient.GetAsync("/V1/Authentication/Authenticator"));

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/FirstAccess", FindContext(userLogin)));

        public async Task<UserResponseLogin> LoginWith2fa(UserLoginWith2fa with2Fa)
        => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/FirstAccess-with2fa", FindContext(with2Fa)));

        public async Task<UserResponseLogin> Register(UserRegister userRegister)
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/Register", FindContext(userRegister)));

        public async Task<ResponseResult> ResetAuthenticator()
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync("/V1/Authentication/ResetAuthenticator", FindContext(null)));
    }
}
