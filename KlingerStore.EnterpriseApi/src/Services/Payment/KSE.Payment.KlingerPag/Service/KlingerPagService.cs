using KSE.Payment.KlingerPag.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace KSE.Payment.KlingerPag.Service
{
    public interface IKlingerPagService
    {
        Task<PaymentResponse> PerformingTransaction(TransactionViewModel transaction);
        Task<PaymentResponse> CapturingTransactionLater(string tid, string key);
        Task<PaymentResponse> CanceledTransaction(string tid, string key);
    }

    public class KlingerPagService : Services, IKlingerPagService
    {
        private readonly HttpClient _httpClient;

        public KlingerPagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://api.pagar.me");
        }

        public async Task<PaymentResponse> CapturingTransactionLater(string tid, string key)
        {
            var @objet = FindContext(new { api_key = key });

            var result = await _httpClient.PostAsync($"1/transactions/{tid}/capture", @objet);

            if (!TreatErrosResponse(result)) return new PaymentResponse { TransactionError = await DeserializeResponse<TransactionError>(result) };

            return new PaymentResponse { TransactionResponse = await DeserializeResponse<TransactionResponse>(result) };
        }

        public async Task<PaymentResponse> CanceledTransaction(string tid, string key)
        {
            var @objet = FindContext(new { api_key = key });

            var result = await _httpClient.PostAsync($"1/transactions/{tid}/refund", @objet);

            if (!TreatErrosResponse(result)) return new PaymentResponse { TransactionError = await DeserializeResponse<TransactionError>(result) };

            return new PaymentResponse { TransactionResponse = await DeserializeResponse<TransactionResponse>(result) };
        }

        public async Task<PaymentResponse> PerformingTransaction(TransactionViewModel transaction)
        {
            var @objet = FindContext(transaction);

            var result = await _httpClient.PostAsync($"1/transactions", @objet);

            if (!TreatErrosResponse(result)) return new PaymentResponse { TransactionError = await DeserializeResponse<TransactionError>(result) };

            return new PaymentResponse { TransactionResponse = await DeserializeResponse<TransactionResponse>(result) };
        }    
    }
}
