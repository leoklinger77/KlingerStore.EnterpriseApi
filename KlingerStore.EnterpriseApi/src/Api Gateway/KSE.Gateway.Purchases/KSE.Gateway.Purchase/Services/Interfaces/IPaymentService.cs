using KSE.Gateway.Purchase.Models.Payment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<TaxaTransactionDTO>> GetAllsTaxa();
    }
}
