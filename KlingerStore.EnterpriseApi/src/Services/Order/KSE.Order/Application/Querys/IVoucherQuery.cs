using KSE.Order.Application.DTO;
using System.Threading.Tasks;

namespace KSE.Order.Application.Querys
{
    public interface IVoucherQuery
    {
        Task<VoucherDTO> GetVoucherPerCode(string code);
    }
}
