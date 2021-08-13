using KSE.WebAppMvc.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContext = new StringContent(JsonSerializer.Serialize(userLogin),Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync("https://localhost:44378/V1/Authentication/FirstAccess", loginContext);

            if (response.IsSuccessStatusCode)
            {                
                return JsonSerializer.Deserialize<UserResponseLogin>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        public async Task<UserResponseLogin> Registro(UserRegister userRegister)
        {
            var userContext = new StringContent(JsonSerializer.Serialize(userRegister), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44378/V1/Authentication/Register", userContext);

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<UserResponseLogin>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }
    }
}
