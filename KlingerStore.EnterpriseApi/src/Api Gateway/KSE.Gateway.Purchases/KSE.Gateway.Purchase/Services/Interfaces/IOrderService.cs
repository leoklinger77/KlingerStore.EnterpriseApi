using KSE.Core.Communication;
using KSE.Gateway.Purchase.Models.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Interfaces
{
    public interface IOrderService
    {
        Task<VoucherDTO> GetVoucherPerCode(string code);
        Task<IEnumerable<OrderDTO>> GetAllOrder();
        Task<OrderDTO> GetLastOrder();
        Task<ResponseResult> FinishOrder(OrderDTO pedido);
    }
}
