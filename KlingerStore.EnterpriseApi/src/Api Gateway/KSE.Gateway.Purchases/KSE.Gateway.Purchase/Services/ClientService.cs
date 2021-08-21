using KSE.Gateway.Purchase.Extensions;
using KSE.Gateway.Purchase.Models.Client;
using KSE.Gateway.Purchase.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services
{
    public class ClientService : Services, IClientService
    {
        private readonly HttpClient _httpClient;

        public ClientService(HttpClient httpClient, IOptions<AppServicesSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.ClientUrl);
        }
        public async Task<AddressDTO> GetAddress()
                => await ReturnResponse<AddressDTO>(await _httpClient.GetAsync($"v1/client/address"));
    }
}
