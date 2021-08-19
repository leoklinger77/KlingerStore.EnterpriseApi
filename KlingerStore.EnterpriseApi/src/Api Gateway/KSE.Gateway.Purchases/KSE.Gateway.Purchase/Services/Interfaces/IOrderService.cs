using KSE.Gateway.Purchase.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Interfaces
{
    public interface IOrderService
    {
        Task<VoucherDTO> GetVoucherPerCode(string code);
    }
}
