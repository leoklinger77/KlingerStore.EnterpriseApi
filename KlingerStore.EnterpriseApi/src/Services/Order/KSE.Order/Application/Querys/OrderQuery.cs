using AutoMapper;
using KSE.Order.Application.DTO;
using KSE.Order.Application.Querys.Interfaces;
using KSE.Order.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Order.Application.Querys
{
    public class OrderQuery : IOrderQuery
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderQuery(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetFindAll(Guid clientId)
        {
            return _mapper.Map<IEnumerable<OrderDTO>>(await _orderRepository.FindAllPerClient(clientId)); 
        }

        public async Task<OrderDTO> GetLastOrder(Guid clientId)
        {
            return _mapper.Map<OrderDTO>(await _orderRepository.GetLastOrder(clientId));
        }
    }
}
