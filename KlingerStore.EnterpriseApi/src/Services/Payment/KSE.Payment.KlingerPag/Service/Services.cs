using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KSE.Payment.KlingerPag
{
    public abstract class Services
    {
        protected StringContent FindContext(object data)
        {
            var serialize = JsonSerializer.Serialize(data);

            return new StringContent(serialize, Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
        {
            var result = await responseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
