using System;
using System.Threading.Tasks;

namespace KSE.Cart.Repository
{
    public interface ICartRepository
    {
        Task<Models.Cart> GetClient(Guid id);
        Task<Models.Cart> Get(Guid id);
        Task<Models.Cart> Create(Models.Cart cart);
        Task Update(Guid id, Models.Cart cart);
        Task Delete(Guid id);
    }
}
