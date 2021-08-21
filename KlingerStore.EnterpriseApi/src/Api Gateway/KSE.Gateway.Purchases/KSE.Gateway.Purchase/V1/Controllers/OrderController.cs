using KSE.Gateway.Purchase.Models.Cart;
using KSE.Gateway.Purchase.Models.Client;
using KSE.Gateway.Purchase.Models.Order;
using KSE.Gateway.Purchase.Services.Interfaces;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.V1.Controllers
{
    [Authorize]
    [Route("v1/Order")]
    public class OrderController : MainController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;

        public OrderController(ICatalogService catalogService, ICartService cartService, IOrderService orderService, IClientService clientService)
        {
            _catalogService = catalogService;
            _cartService = cartService;
            _orderService = orderService;
            _clientService = clientService;
        }

        [HttpGet("FindAll")]
        public async Task<IActionResult> GetAllOrder()
        {
            var pedidos = await _orderService.GetAllOrder();

            return pedidos == null ? NotFound() : CustomResponse(pedidos);
        }

        [HttpPost]
        [Route("createOrder")]
        public async Task<IActionResult> CreateOrder(OrderDTO pedido)
        {
            var carrinho = await _cartService.GetCart();
            var produtos = await _catalogService.GetItens(carrinho.Itens.Select(p => p.ProductId));
            var endereco = await _clientService.GetAddress();

            if (!await ValidCartProduct(carrinho, produtos)) return CustomResponse();

            MappingOrder(carrinho, endereco, pedido);

            return CustomResponse(await _orderService.FinishOrder(pedido));
        }

        [HttpGet("lastOrder")]
        public async Task<IActionResult> LastOrder()
        {
            var order = await _orderService.GetLastOrder();
            if (order is null)
            {
                AddErros("Pedido não encontrado!");
                return CustomResponse();
            }

            return CustomResponse(order);
        }       

        private async Task<bool> ValidCartProduct(CartDTO carrinho, IEnumerable<ItemProductDTO> produtos)
        {
            if (carrinho.Itens.Count != produtos.Count())
            {
                var itensIndisponiveis = carrinho.Itens.Select(c => c.ProductId).Except(produtos.Select(p => p.Id)).ToList();

                foreach (var itemId in itensIndisponiveis)
                {
                    var itemCarrinho = carrinho.Itens.FirstOrDefault(c => c.ProductId == itemId);
                    AddErros($"O item {itemCarrinho.Name} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }

                return false;
            }

            foreach (var itemCarrinho in carrinho.Itens)
            {
                var produtoCatalogo = produtos.FirstOrDefault(p => p.Id == itemCarrinho.ProductId);

                if (produtoCatalogo.Value != itemCarrinho.Value)
                {
                    var msgErro = $"O produto {itemCarrinho.Name} mudou de valor (de: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCarrinho.Value)} para: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", produtoCatalogo.Value)}) desde que foi adicionado ao carrinho.";

                    AddErros(msgErro);

                    var responseRemover = await _cartService.DeleteItemCart(itemCarrinho.ProductId);
                    if (ResponseHasError(responseRemover))
                    {
                        AddErros($"Não foi possível remover automaticamente o produto {itemCarrinho.Name} do seu carrinho, _" +
                                                   "remova e adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    itemCarrinho.Value = produtoCatalogo.Value;
                    var responseAdicionar = await _cartService.AddItemCart(itemCarrinho);

                    if (ResponseHasError(responseAdicionar))
                    {
                        AddErros($"Não foi possível atualizar automaticamente o produto {itemCarrinho.Name} do seu carrinho, _" +
                                                   "adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    ClearErros();
                    AddErros(msgErro + " Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }

        private void MappingOrder(CartDTO carrinho, AddressDTO endereco, OrderDTO pedido)
        {
            pedido.VoucherCode = carrinho.Voucher?.Code;
            pedido.VoucherUsed = carrinho.VoucherUsed;
            pedido.TotalValue = carrinho.TotalValue;
            pedido.Discount = carrinho.Discount;
            pedido.Itens = carrinho.Itens;

            pedido.Address = endereco;
        }
    }
}
