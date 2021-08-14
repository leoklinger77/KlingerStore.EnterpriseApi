using KSE.WebAppMvc.Extensions.Exceptions;
using System.Net.Http;

namespace KSE.WebAppMvc.Services
{
    public abstract class Services
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
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
