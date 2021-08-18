using KSE.Gateway.Purchase.Extensions;
using KSE.Gateway.Purchase.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
