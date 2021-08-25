using KSE.Core.Interfaces;
using KSE.Payment.Interfaces;
using KSE.Payment.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Payment.Data.Repository
{
    public class TaxaTransactionRepository : ITaxaTransactionRepository
    {
        private readonly PaymentContext _paymentContext;

        public TaxaTransactionRepository(PaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }

        public IUnitOfWork UnitOfWork => _paymentContext;        

        public async Task<IEnumerable<TaxaTransaction>> GetFindAll()
        {
            return await _paymentContext.TaxaTransaction.AsNoTracking().Where(x => x.Active == true).ToListAsync();
        }

        public void Dispose()
        {
            _paymentContext?.DisposeAsync();
        }
    }
}
