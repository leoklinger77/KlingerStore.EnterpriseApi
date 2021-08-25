using KSE.Core.Interfaces;
using KSE.Order.Domain.Domain;
using KSE.Order.Domain.Domain.Enumerations;
using KSE.Order.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Order.Infrastructure.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;

        public OrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public IUnitOfWork UnitOfWork => _orderContext;
        public async Task<OrderItem> GetItemPerOrderId(Guid orderId, Guid productId)
        {
            return await _orderContext.OrderItem
                .AsNoTracking()
                .Where(x => x.Id == orderId && x.ProductId == productId)
                .FirstOrDefaultAsync();
        }
        public async Task<Domain.Domain.Order> GetById(Guid id)
        {
            return await _orderContext.Order
                .Include(x => x.Itens)
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Domain.Domain.Order> GetLastOrder(Guid id)
        {
            return await _orderContext.Order
                .Include(x => x.Itens)
                .Include(x => x.Address)
                .AsNoTracking()
                .Where(x => x.ClientId == id && x.OrderStatus == OrderStatus.Authorized)
                .OrderByDescending(x => x.InsertDate)
                .FirstOrDefaultAsync();
        }
        public async Task<Domain.Domain.Order> GetLastOrderAuthorize()
        {
            return await _orderContext.Order
                .Include(x => x.Itens)
                .AsNoTracking()
                .Where(x => x.OrderStatus == OrderStatus.Authorized)
                .OrderByDescending(x => x.InsertDate)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderItem> GetItemId(Guid id)
        {
            return await _orderContext.OrderItem

                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Domain.Domain.Order>> FindAllPerClient(Guid clientId)
        {
            return await _orderContext.Order
                .Include(x => x.Itens)
                .Include(x => x.Address)
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .ToListAsync();
        }
        public async Task Insert(Domain.Domain.Order order)
        {
            await _orderContext.Order.AddAsync(order);
        }

        public void Update(Domain.Domain.Order order)
        {
            _orderContext.Order.Update(order);
        }
        public void Dispose()
        {
            _orderContext?.DisposeAsync();
        }


    }
}
