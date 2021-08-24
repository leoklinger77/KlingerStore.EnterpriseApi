using KSE.Core.DomainObjets;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Core.Messages.IntegrationEvents.Order;
using KSE.MessageBus;
using KSE.Payment.KlingerPag.Models;
using KSE.Payment.Models.Enums;
using KSE.Payment.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Payment.Services.Handler
{
    public class PaymentIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PaymentIntegrationHandler(
                            IServiceProvider serviceProvider,
                            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        private void SetResponder()
        {
            _bus.RespondAsync<OrderStartIntegrationEvent, ResponseMessage>(async request =>
                await AutorizarPagamento(request));
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderCanceledIntegrationEvent>("PedidoCancelado", async request =>
            await CancelarPagamento(request));

            _bus.SubscribeAsync<OrderWrittenOffFromStockIntegrationEvent>("PedidoBaixadoEstoque", async request =>
            await CapturarPagamento(request));
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            SetSubscribers();
            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> AutorizarPagamento(OrderStartIntegrationEvent message)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPaymentService>();


                List<ItemViewModel> itens = new List<ItemViewModel>();

                foreach (var item in message.Itens)
                {
                    itens.Add(new ItemViewModel
                    {
                        id = item.Id.ToString(),
                        quantity = item.Quantity,
                        tangible = item.Tangible,
                        title = item.Title.ToString(),
                        unit_price = item.Unit_price.ToString().Remove(item.Unit_price.ToString().Length - 3, 1)
                    });
                }

                var pagamento = new Models.Payment
                {
                    OrderId = message.OrderId,
                    TypePayment = (TypePayment)message.TypeOrder,
                    Value = message.Value,
                    CreditCart = new Models.CreditCart(
                        message.NameCard, message.NumberCard, message.ExpirationCard, message.CVV),
                    City = message.City,
                    ClientDocument = message.ClientDocument,
                    ClientEmail = message.ClientEmail,
                    ClientId = message.ClientId,
                    ClientName = message.ClientName,
                    ClientPhone = message.ClientPhone,
                    Itens = itens,
                    Neighborhood = message.Neighborhood,
                    Number = message.Number,
                    State = message.State,
                    Street = message.Street,
                    Zipcode = message.Zipcode
                };

                var response = await pagamentoService.AutorizarPagamento(pagamento);

                return response;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task CancelarPagamento(OrderCanceledIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                var response = await pagamentoService.CancelarPagamento(message.OrderId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao cancelar pagamento do pedido {message.OrderId}");
            }
        }

        private async Task CapturarPagamento(OrderWrittenOffFromStockIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                var response = await pagamentoService.CapturarPagamento(message.OrderId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao capturar pagamento do pedido {message.OrderId}");

                await _bus.PublishAsync(new PaidOrderIntegrationEvent(message.ClientId, message.OrderId));
            }
        }
    }
}
