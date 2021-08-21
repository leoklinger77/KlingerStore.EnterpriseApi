using KSE.Core.Interfaces;
using KSE.Order.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Order.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Domain.Order>
    {
        Task<Domain.Order> GetById(Guid id);
        Task<Domain.Order> GetLastOrder(Guid id);
        
        Task<IEnumerable<Domain.Order>> FindAllPerClient(Guid clientId);
        Task Insert(Domain.Order order);
        void Update(Domain.Order order);

        Task<OrderItem> GetItemId(Guid id);
        Task<OrderItem> GetItemPerOrderId(Guid orderId, Guid productId);
    }
}
