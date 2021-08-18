using KSE.Core.Communication;
using KSE.Gateway.Purchase.Models.Cart;
using System;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCart();        
        Task<ResponseResult> AddItemCart(ItemCartDTO product);
        Task<ResponseResult> UpdateItemCart(Guid productId, ItemCartDTO product);
        Task<ResponseResult> DeleteItemCart(Guid productId);
    }
}
