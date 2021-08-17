using KSE.WebAppMvc.Models;
using System;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart();
        Task<ResponseResult> AddItemCart(ItemProductViewModel product);
        Task<ResponseResult> UpdateItemCart(Guid productId, ItemProductViewModel product);
        Task<ResponseResult> RemoveItemCart(Guid productId);
    }
}
