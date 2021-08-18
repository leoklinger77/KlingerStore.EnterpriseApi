using KSE.Core.Communication;
using KSE.Gateway.Purchase.Extensions;
using KSE.Gateway.Purchase.Models.Cart;
using KSE.Gateway.Purchase.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services
{
    public class CartService : Services, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.CartUrl);
        }

        public async Task<CartDTO> GetCart() 
            => await ReturnResponse<CartDTO>(await _httpClient.GetAsync("V1/Cart"));
        public async Task<ResponseResult> AddItemCart(ItemCartDTO product) 
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync($"V1/Cart", FindContext(product)));

        public async Task<ResponseResult> UpdateItemCart(Guid productId, ItemCartDTO product) 
            => await ReturnResponse<ResponseResult>(await _httpClient.PutAsync($"V1/Cart/{productId}", FindContext(product)));

        public async Task<ResponseResult> DeleteItemCart(Guid productId) 
            => await ReturnResponse<ResponseResult>(await _httpClient.DeleteAsync($"V1/Cart/{productId}"));

       
    }
}
