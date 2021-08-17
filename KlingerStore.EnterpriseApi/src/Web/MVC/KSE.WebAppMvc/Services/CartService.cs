using KSE.WebAppMvc.Extensions;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public class CartService : Services, ICartService
    {
        private readonly HttpClient _httpClient;
        public CartService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
            _httpClient = httpClient;
        }
        public async Task<CartViewModel> GetCart()
            => await ReturnResponse<CartViewModel>(await _httpClient.GetAsync($"V1/Cart"));

        public async Task<ResponseResult> AddItemCart(ItemProductViewModel product)
        {
            var result = await _httpClient.PostAsync("/V1/Cart", FindContext(product));

            return await ReturnResponse<ResponseResult>(result);
        }
            

        public async Task<ResponseResult> RemoveItemCart(Guid productId) 
            => await ReturnResponse<ResponseResult>(await _httpClient.DeleteAsync($"/V1/Cart/{productId}"));

        public async Task<ResponseResult> UpdateItemCart(Guid productId, ItemProductViewModel product) 
            => await ReturnResponse<ResponseResult>(await _httpClient.PutAsync($"/V1/Cart/{productId}", FindContext(product)));
    }
}
