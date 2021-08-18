using KSE.Gateway.Purchase.Extensions;
using KSE.Gateway.Purchase.Models.Cart;
using KSE.Gateway.Purchase.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services
{
    public class CatalogService : Services, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.CatalogUrl);
        }

        public async Task<ItemProductDTO> FindById(Guid id)
            => await ReturnResponse<ItemProductDTO>(await _httpClient.GetAsync($"V1/Catalog/Product/{id}"));
    }
}
