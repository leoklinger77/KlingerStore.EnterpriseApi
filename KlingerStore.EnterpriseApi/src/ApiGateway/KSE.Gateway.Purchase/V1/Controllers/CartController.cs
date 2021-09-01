using KSE.Gateway.Purchase.Models.Cart;
using KSE.Gateway.Purchase.Services.gRPC;
using KSE.Gateway.Purchase.Services.Interfaces;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.V1.Controllers
{
    [Authorize]
    [Route("V1/Cart")]
    public class CartController : MainController
    {
        private readonly ICartGrpcService _cartGrpcService;
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService,
                              ICatalogService catalogService, IOrderService orderService, ICartGrpcService cartGrpcService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
            _orderService = orderService;
            _cartGrpcService = cartGrpcService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            return CustomResponse(await _cartGrpcService.GetCart());
        }

        [HttpGet("QuantityCart")]
        public int GetQuantityCart()
        {
            return (int)_cartGrpcService.GetCart().Result.Itens?.Sum(x => x.Quantity);
        }

        [HttpPost]
        public async Task<IActionResult> PostAddItemCart(ItemCartDTO itemProduct)
        {
            var product = await _catalogService.FindById(itemProduct.ProductId);
            await ValidItemCart(product, itemProduct.Quantity);
            if (!OperationVation()) return CustomResponse();

            itemProduct.Name = product.Name;
            itemProduct.Value = product.Value;
            itemProduct.Image = product.Image;

            ResponseHasError(await _cartService.AddItemCart(itemProduct));

            return CustomResponse();
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> PuttAddItemCart(Guid productId, ItemCartDTO itemProduct)
        {

            await ValidItemCart(await _catalogService.FindById(productId), itemProduct.Quantity);
            if (!OperationVation()) return CustomResponse();

            ResponseHasError(await _cartService.UpdateItemCart(productId, itemProduct));

            return CustomResponse();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteItemCart(Guid productId)
        {
            var product = await _catalogService.FindById(productId);
            if (product is null)
            {
                AddErros("Produto inexistente.");
            }

            return CustomResponse(await _cartService.DeleteItemCart(productId));
        }

        [HttpPost("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher([FromBody] string voucherCode)
        {
            var voucher = await _orderService.GetVoucherPerCode(voucherCode);
            if (voucher is null)
            {
                AddErros("Voucher inválido ou não encontrado!");
                return CustomResponse();
            }
            var response = await _cartService.ApplyVoucherCart(voucher);
            return CustomResponse(response);
        }

        private async Task ValidItemCart(ItemProductDTO product, int quantity)
        {
            if (product is null) AddErros("Produto inexistente.");
            if (quantity < 1) AddErros($"Escolha ao menos uma unidade do produto {product.Name}");

            var cart = await _cartService.GetCart();
            var itemCart = cart.Itens.FirstOrDefault(x => x.ProductId == product.Id);

            if (itemCart != null && itemCart.Quantity + quantity > product.QuantityStock)
            {
                AddErros($"O produto {product.Name} possui {product.Quantity} unidades em estoqu, vc selecionou {quantity} unidades.");
                return;
            }

            if (quantity > product.QuantityStock)
            {
                AddErros($"O produto {product.Name} possui {product.Quantity} unidades em estoqu, vc selecionou {quantity} unidades.");
            }
        }
    }
}
