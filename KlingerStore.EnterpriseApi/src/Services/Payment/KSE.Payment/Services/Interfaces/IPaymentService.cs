using KSE.Core.Messages.IntegrationEvents;
using System;
using System.Threading.Tasks;

namespace KSE.Payment.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseMessage> AutorizarPagamento(Models.Payment payment);        
        Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
    }
}
