using KSE.Core.Interfaces;
using KSE.Payment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Payment.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void InsertPayment(Models.Payment pagamento)
        {
            _context.Payment.Add(pagamento);
        }

        public void InsertTransaction(Models.Transaction transacao)
        {
            _context.Transaction.Add(transacao);
        }

        public async Task<Models.Payment> FindPaymentPerOrderId(Guid pedidoId)
        {
            return await _context.Payment.AsNoTracking()
                .FirstOrDefaultAsync(p => p.OrderId == pedidoId);
        }

        public async Task<IEnumerable<Models.Transaction>> FindTransactionPerOrderId(Guid pedidoId)
        {
            return await _context.Transaction.AsNoTracking()
                .Where(t => t.Payment.OrderId == pedidoId).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
