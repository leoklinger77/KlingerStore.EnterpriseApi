using KSE.Core.Messages.IntegrationEvents;
using System;
using System.Threading.Tasks;

namespace KSE.Payment.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseMessage> AutorizarPagamento(Payment.Models.Payment payment);
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
        Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
    }
}
