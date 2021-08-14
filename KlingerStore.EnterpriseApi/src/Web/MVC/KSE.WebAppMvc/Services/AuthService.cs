using KSE.WebAppMvc.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public class AuthService : Services, IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContext = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44378/V1/Authentication/FirstAccess", loginContext);

            var rsrs = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!TratarErrosResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), option)
                };
            }

            return JsonSerializer.Deserialize<UserResponseLogin>(await response.Content.ReadAsStringAsync(), option);
        }

        public async Task<UserResponseLogin> Registro(UserRegister userRegister)
        {
            var userContext = new StringContent(JsonSerializer.Serialize(userRegister), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44378/V1/Authentication/Register", userContext);

            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!TratarErrosResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), option)
                };
            }

            return JsonSerializer.Deserialize<UserResponseLogin>(await response.Content.ReadAsStringAsync(), option);
        }
    }
}
