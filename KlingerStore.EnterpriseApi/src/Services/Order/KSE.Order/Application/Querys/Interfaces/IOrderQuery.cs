using KSE.Order.Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Order.Application.Querys.Interfaces
{
    public interface IOrderQuery
    {
        Task<OrderDTO> GetLastOrder(Guid clientId);
        Task<IEnumerable<OrderDTO>> GetFindAll(Guid clientId);
        Task<OrderDTO> GetOrderAuthorize();
    }
}
