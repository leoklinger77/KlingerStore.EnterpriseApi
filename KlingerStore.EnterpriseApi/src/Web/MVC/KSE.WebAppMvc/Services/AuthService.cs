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

        public async Task<UserResponseLogin> Login(UserLogin userLogin) 
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/FirstAccess", FindContext(userLogin)));

        public async Task<UserResponseLogin> Register(UserRegister userRegister)
            => await ReturnResponse<UserResponseLogin>(await _httpClient.PostAsync("/V1/Authentication/Register", FindContext(userRegister)));
        
    }
}
