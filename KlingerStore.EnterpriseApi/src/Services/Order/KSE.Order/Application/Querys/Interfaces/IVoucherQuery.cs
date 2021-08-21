using KSE.Order.Application.DTO;
using System.Threading.Tasks;

namespace KSE.Order.Application.Querys.Interfaces
{
    public interface IVoucherQuery
    {
        Task<VoucherDTO> GetVoucherPerCode(string code);
    }
}
