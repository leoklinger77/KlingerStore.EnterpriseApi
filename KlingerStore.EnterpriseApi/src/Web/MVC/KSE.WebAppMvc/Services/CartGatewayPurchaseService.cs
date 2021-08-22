using KSE.Core.Communication;
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
    public class CartGatewayPurchaseService : Services, IGatewayPurchaseService
    {
        private readonly HttpClient _httpClient;
        public CartGatewayPurchaseService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.GatewayPurchaseUrl);
            _httpClient = httpClient;
        }
        public async Task<CartViewModel> GetCart()
            => await ReturnResponse<CartViewModel>(await _httpClient.GetAsync($"V1/Cart"));
        public async Task<int> GetQuantityCart()
            => await ReturnResponse<int>(await _httpClient.GetAsync($"/V1/Cart/QuantityCart"));

        public async Task<ResponseResult> AddItemCart(CartItemViewModel product)
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync($"/V1/Cart", FindContext(product)));

        public async Task<ResponseResult> DeleteItemCart(Guid productId)
            => await ReturnResponse<ResponseResult>(await _httpClient.DeleteAsync($"/V1/Cart/{productId}"));

        public async Task<ResponseResult> UpdateItemCart(Guid productId, CartItemViewModel product)
            => await ReturnResponse<ResponseResult>(await _httpClient.PutAsync($"/V1/Cart/{productId}", FindContext(product)));

        public async Task<ResponseResult> ApplyVoucher(string code)
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync($"/V1/Cart/apply-voucher", FindContext(code)));

        public async Task<ResponseResult> FinishOrder(TransactionOrderViewModel transactionOrder)
            => await ReturnResponse<ResponseResult>(await _httpClient.PostAsync($"/V1/Order/createOrder", FindContext(transactionOrder)));

        public async Task<IEnumerable<OrderViewModel>> GetFindAllOrder()
            => await ReturnResponse<IEnumerable<OrderViewModel>>(await _httpClient.GetAsync($"/V1/Order/listorder"));

        public async Task<OrderViewModel> GetLastOrder()
            => await ReturnResponse<OrderViewModel>(await _httpClient.GetAsync($"/V1/Order/lastorder"));
    }
}
