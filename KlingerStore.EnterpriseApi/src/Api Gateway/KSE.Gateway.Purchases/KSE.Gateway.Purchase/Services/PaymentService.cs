using KSE.Gateway.Purchase.Extensions;
using KSE.Gateway.Purchase.Models.Payment;
using KSE.Gateway.Purchase.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services
{
    public class PaymentService : Services, IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.PaymentUrl);
        }

        public async Task<IEnumerable<TaxaTransactionDTO>> GetAllsTaxa()
            => await ReturnResponse<IEnumerable<TaxaTransactionDTO>>(await _httpClient.GetAsync($"v1/Payment/Taxa"));
    }
}
