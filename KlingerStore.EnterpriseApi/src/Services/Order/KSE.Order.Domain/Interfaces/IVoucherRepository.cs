using KSE.Core.Interfaces;
using KSE.Order.Domain.Domain;
using System.Threading.Tasks;

namespace KSE.Order.Domain.Interfaces
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> GetVoucherPerCode(string code);
    }
}
