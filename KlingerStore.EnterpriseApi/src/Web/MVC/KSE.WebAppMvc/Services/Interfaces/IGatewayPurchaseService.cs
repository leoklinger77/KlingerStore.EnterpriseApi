using KSE.Core.Communication;
using KSE.WebAppMvc.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface IGatewayPurchaseService
    {
        Task<CartViewModel> GetCart();
        Task<int> GetQuantityCart();
        Task<ResponseResult> AddItemCart(CartItemViewModel product);
        Task<ResponseResult> UpdateItemCart(Guid productId, CartItemViewModel product);
        Task<ResponseResult> DeleteItemCart(Guid productId);
        Task<ResponseResult> ApplyVoucher(string code);
        Task<ResponseResult> FinishOrder(TransactionOrderViewModel transactionOrder);
        Task<IEnumerable<OrderViewModel>> GetListOrder();
    }
}
