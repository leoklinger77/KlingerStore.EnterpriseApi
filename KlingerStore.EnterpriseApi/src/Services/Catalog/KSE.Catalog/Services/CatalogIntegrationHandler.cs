using KSE.Catalog.Extensions.Exceptions;
using KSE.Catalog.Interfaces;
using KSE.Catalog.Models;
using KSE.Core.Messages.IntegrationEvents.Order;
using KSE.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Catalog.Services
{
    public class CatalogIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CatalogIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderAuthorizeIntegrationEvent>("PedidoAutorizado", async request =>
                await BaixarEstoque(request));
        }

        private async Task BaixarEstoque(OrderAuthorizeIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var produtosComEstoque = new List<Product>();
                var produtoRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));
                var produtos = await produtoRepository.FindAllProductPerIds(idsProdutos);

                if (produtos.Count() != message.Itens.Count)
                {
                    CancelarPedidoSemEstoque(message);
                    return;
                }

                foreach (var produto in produtos)
                {
                    var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;
                    
                    if (produto.IsAvailable(quantidadeProduto))
                    {
                        produto.DebitStock(quantidadeProduto);
                        produtosComEstoque.Add(produto);
                    }
                }

                if (produtosComEstoque.Count != message.Itens.Count)
                {
                    CancelarPedidoSemEstoque(message);
                    return;
                }

                foreach (var produto in produtosComEstoque)
                {
                    produtoRepository.Update(produto);
                }

                if (!await produtoRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"Problemas ao atualizar estoque do pedido {message.OrderId}");
                }

                var pedidoBaixado = new OrderWrittenOffFromStockIntegrationEvent(message.ClientId, message.OrderId);
                await _bus.PublishAsync(pedidoBaixado);
            }
        }

        public async void CancelarPedidoSemEstoque(OrderAuthorizeIntegrationEvent message)
        {
            var pedidoCancelado = new OrderCanceledIntegrationEvent(message.ClientId, message.OrderId);
            var pedidoEstornado= new OrderRefoundIntegrationEvent(message.ClientId, message.OrderId);
            await _bus.PublishAsync(pedidoEstornado);
            await _bus.PublishAsync(pedidoCancelado);
        }
    }
}