using AutoMapper;
using FluentValidation.Results;
using KSE.Core.Messages;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Core.Messages.IntegrationEvents.Order;
using KSE.MessageBus;
using KSE.Order.Application.Events;
using KSE.Order.Domain.Domain;
using KSE.Order.Domain.Interfaces;
using MediatR;
using System;
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
        private readonly IMessageBus _bus;
        public OrderCommandHandler(IMapper mapper, IVoucherRepository voucherRepository, IOrderRepository orderRepository, IMessageBus bus)
        {
            _mapper = mapper;
            _voucherRepository = voucherRepository;
            _orderRepository = orderRepository;
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(CreatedOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var order = MapperComandAsync(message);

            if (!await ApplyVoucher(message, order)) return ValidationResult;

            order.IsValid();

            if (!ValidOrder(order)) return ValidationResult;

            if (!await ProccessPayment(order, message)) return ValidationResult;

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

            if (order.TotalValue != orderValueOriginal)
            {
                AddError("O valor total do pedido não confere como cálculo do pedido.");
                return false;
            }

            if (order.Discount != orderDiscount)
            {
                AddError("O valor total não confere com o cálculo do pedido.");
                return false;
            }
            return true;
        }

        public async Task<bool> ProccessPayment(Domain.Domain.Order order, CreatedOrderCommand message)
        {
            try
            {
                List<OrderStartItems> itens = new List<OrderStartItems>();

                foreach (var item in order.Itens)
                {
                    itens.Add(new OrderStartItems
                    {
                        Id = item.ProductId.ToString(),
                        Quantity = item.Quantity,
                        Title = item.Name,
                        Tangible = true,
                        Unit_price = item.Value.ToString()
                    });
                }

                var pedidoIniciado = new OrderStartIntegrationEvent
                {
                    OrderId = order.Id,
                    ClientId = order.ClientId,
                    TypeOrder = 1,
                    Value = order.TotalValue,
                    NameCard = message.NameCart,
                    NumberCard = message.NumberCart,
                    ExpirationCard = message.ExpirationCArt,
                    CVV = message.CVVCart,

                    ClientName = "leandro Klinger",
                    ClientEmail = "leandro@gmail.com",
                    ClientDocument = "36018556820",
                    ClientPhone = "+5511954665152",

                    City = order.Address.City,
                    State = order.Address.State,
                    Neighborhood = order.Address.District,
                    Street = order.Address.Street,
                    Number = order.Address.Number,
                    Zipcode = order.Address.ZipCode,

                    Itens = itens
                };

                var result = await _bus
                    .RequestAsync<OrderStartIntegrationEvent, ResponseMessage>(pedidoIniciado);

                if (result.ValidationResult.IsValid) return true;

                foreach (var erro in result.ValidationResult.Errors)
                {
                    AddError(erro.ErrorMessage);
                }

                return false;
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
}

