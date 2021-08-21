using AutoMapper;
using FluentValidation.Results;
using KSE.Core.Messages;
using KSE.Order.Application.Events;
using KSE.Order.Domain.Domain;
using KSE.Order.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Order.Application.Commands
{
    public class OrderCommandHandler : CommandHandler,
        IRequestHandler<CreatedOrderCommand, ValidationResult>
    {
        private readonly IMapper _mapper;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderCommandHandler(IMapper mapper, IVoucherRepository voucherRepository, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _voucherRepository = voucherRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ValidationResult> Handle(CreatedOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var order = MapperComandAsync(message);

            if(!await ApplyVoucher(message, order)) return ValidationResult;

            order.IsValid();

            if (!ValidOrder(order)) return ValidationResult;

            if (!ProccessPayment(order)) return ValidationResult;

            order.AuthorizeOrder();

            order.AddEvent(new OrderPlacedEvent(order.Id, order.ClientId));

            await _orderRepository.Insert(order);

            return await PersistData(_orderRepository.UnitOfWork);
        }

        private Domain.Domain.Order MapperComandAsync(CreatedOrderCommand message)
        {
            var address = new ShippingAddress(message.Address.Street,
                                              message.Address.Number,
                                              message.Address.Complement,
                                              message.Address.District,
                                              message.Address.ZipCode,
                                              message.Address.City,
                                              message.Address.State);

            var order = new Domain.Domain.Order(message.ClientId, message.TotalValue,
                                                    _mapper.Map<List<OrderItem>>(message.Itens), message.VoucherUsed, message.Discount);

            order.AssignAddress(address);
            return order;
        }

        private async Task<bool> ApplyVoucher(CreatedOrderCommand message, Domain.Domain.Order order)
        {
            if (!message.VoucherUsed) return true;

            var voucher = await _voucherRepository.GetVoucherPerCode(message.VoucherCode);
            if (voucher is null)
            {
                AddError("O voucher informado não existe!");
                return false;
            }            
            if (!voucher.IsValid())
            {
                voucher.ValidationResult.Errors.ToList().ForEach(x => AddError(x.ErrorMessage));
            }
            order.AssignVoucher(voucher);
            voucher.DebitBalance();

            _voucherRepository.Update(voucher);
            return true;
        }

        private bool ValidOrder(Domain.Domain.Order order)
        {
            var orderValueOriginal = order.TotalValue;
            var orderDiscount = order.Discount;

            order.CalculateTotalOrder();

            if(order.TotalValue != orderValueOriginal)
            {
                AddError("O valor total do pedido não confere como cálculo do pedido.");
                return false;
            }

            if(order.Discount != orderDiscount)
            {
                AddError("O valor total não confere com o cálculo do pedido.");
                return false;
            }
            return true;
        }

        public bool ProccessPayment(Domain.Domain.Order order)
        {
            return true;
        }
    }
}

