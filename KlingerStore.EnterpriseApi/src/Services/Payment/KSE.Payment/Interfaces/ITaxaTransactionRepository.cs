using KSE.Core.Interfaces;
using KSE.Payment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Payment.Interfaces
{
    public interface ITaxaTransactionRepository : IRepository<TaxaTransaction>
    {
        Task<IEnumerable<TaxaTransaction>> GetFindAll();
    }
}
