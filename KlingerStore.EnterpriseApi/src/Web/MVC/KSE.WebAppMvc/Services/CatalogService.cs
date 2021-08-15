using KSE.WebAppMvc.Extensions;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public class CatalogService : Services, ICatalogService
    {
        private readonly HttpClient _httpClient;
        public CatalogService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ProductViewModel>> FindAll() 
            => await ReturnResponse<IEnumerable<ProductViewModel>>(await _httpClient.GetAsync("/V1/Catalog/Product"));



        public async Task<ProductViewModel> FindById(Guid id)
            => await ReturnResponse<ProductViewModel>(await _httpClient.GetAsync($"V1/Catalog/Product/{id}"));

    }
}
