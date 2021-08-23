using KSE.Core.Interfaces;
using KSE.Payment.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace KSE.Payment.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment.Models.Payment>
    {
        void InsertPayment(Payment.Models.Payment pagamento);
        void InsertTransaction(Transaction transacao);
        Task<Payment.Models.Payment> FindPaymentPerOrderId(Guid pedidoId);
        Task<IEnumerable<Transaction>> FindTransactionPerOrderId(Guid pedidoId);
    }
}