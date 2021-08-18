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
            => await ReturnResponse<AddressViewModel>(await _httpClient.GetAsync("/V1/Client"));
        
        public async Task<AddressViewModel> GetAddress()
            => await ReturnResponse<AddressViewModel>(await _httpClient.GetAsync("/V1/Client/Address"));
    }
}
