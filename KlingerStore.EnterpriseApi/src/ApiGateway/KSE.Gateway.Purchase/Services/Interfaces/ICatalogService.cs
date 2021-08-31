using KSE.Gateway.Purchase.Models.Cart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<ItemProductDTO> FindById(Guid id);
        Task<IEnumerable<ItemProductDTO>> GetItens(IEnumerable<Guid> productId);
    }
}
