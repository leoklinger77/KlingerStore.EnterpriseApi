using KSE.Core.Communication;
using KSE.Gateway.Purchase.Extensions;
using KSE.Gateway.Purchase.Models.Order;
using KSE.Gateway.Purchase.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services
{
    public class OrderService : Services, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.OrderUrl);
        }

        public async Task<ResponseResult> FinishOrder(OrderDTO order)
        {
            var response = await _httpClient.PostAsync($"v1/Order", FindContext(order));

            if (!TreatErrosResponse(response)) return await DeserializeResponse<ResponseResult>(response);

            return new ResponseResult();
        }



        public async Task<IEnumerable<OrderDTO>> GetAllOrder()
            => await ReturnResponse<IEnumerable<OrderDTO>>(await _httpClient.GetAsync($"v1/Order/list-order"));

        public async Task<OrderDTO> GetLastOrder()
            => await ReturnResponse<OrderDTO>(await _httpClient.GetAsync($"v1/Order/last-order"));

        public async Task<VoucherDTO> GetVoucherPerCode(string code)
            => await ReturnResponse<VoucherDTO>(await _httpClient.GetAsync($"v1/voucher/{code}"));
    }
}
