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
    public class ClientService : Services, IClientService
    {
        private readonly HttpClient _httpClient;
        public ClientService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.ClientUrl);
            _httpClient = httpClient;
        }
        public async Task<ClientViewModel> GetClient()
            => await ReturnResponse<ClientViewModel>(await _httpClient.GetAsync("/V1/Client"));
        
        public async Task<AddressViewModel> GetAddress()
            => await ReturnResponse<AddressViewModel>(await _httpClient.GetAsync("/V1/Client/Address"));

        public async Task<ResponseResult> CreateAddress(AddressViewModel address)
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync("/V1/Client/Address", FindContext(address)));

        public async Task<ResponseResult> UpdateClient(ClientViewModel client)
        {
           var response =  await _httpClient.PostAsync("/V1/Client/Profile", FindContext(client));

            if (!TreatErrosResponse(response))
            {
                return await DeserializeResponse<ResponseResult>(response);
            }

            return ReturnOk();            
        }
            
    }
}
