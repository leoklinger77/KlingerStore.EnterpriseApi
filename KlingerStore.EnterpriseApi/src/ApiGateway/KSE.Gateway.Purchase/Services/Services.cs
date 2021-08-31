using KSE.Core.Communication;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services
{
    public abstract class Services
    {
        protected StringContent FindContext(object data)
        {
            return new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
        {
            try
            {
                var result = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (System.Exception e)
            {

                throw;
            }

        }
        protected bool TreatErrosResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
        protected async Task<dynamic> ReturnResponse<T>(HttpResponseMessage response)
        {
            if (!TreatErrosResponse(response))
            {
                return null;
            }

            if (await ResponseIsNull(response)) return new ResponseResult();

            return await DeserializeResponse<T>(response);
        }
        private async Task<bool> ResponseIsNull(HttpResponseMessage responseMessage)
            => string.IsNullOrEmpty(await responseMessage.Content.ReadAsStringAsync());
    }
}
