using KSE.WebAppMvc.Extensions.Exceptions;
using KSE.WebAppMvc.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public abstract class Services
    {        
        protected StringContent FindContext(object data)
        {
            return  new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }
        
        protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
        {   
            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task<bool> ResponseIsNull(HttpResponseMessage responseMessage) 
            => string.IsNullOrEmpty(await responseMessage.Content.ReadAsStringAsync());


        protected async Task<dynamic> ReturnResponse<T>(HttpResponseMessage response)
        {
            if (!TreatErrosResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeResponse<ResponseResult>(response)
                };
            }

            if (await ResponseIsNull(response)) return new ResponseResult();

            return await DeserializeResponse<T>(response);
        }

        protected bool TreatErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:                    
                case 403:                    
                case 404:                    
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;                
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
