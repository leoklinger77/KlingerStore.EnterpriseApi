using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : MainController
    {
        private readonly IGatewayPurchaseService _cartService;

        public CartController(IGatewayPurchaseService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _cartService.GetCart());
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemCart(CartItemViewModel item)
        {
            if (HasErrorResponse(await _cartService.AddItemCart(item))) return View("Index", await _cartService.GetCart());

            return RedirectToAction(nameof(Index));
        }
        [HttpPost("BuyNow")]
        public async Task<IActionResult> CartBuyNow(CartItemViewModel item)
        {
            if (HasErrorResponse(await _cartService.AddItemCart(item))) return View("Index", await _cartService.GetCart());

            return RedirectToAction("ShippingAddress","Order");
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            if (HasErrorResponse(await _cartService.UpdateItemCart(productId,
                                                                    new CartItemViewModel
                                                                    {
                                                                        ProductId = productId,
                                                                        Quantity = quantity
                                                                    })))
                return View("Index", await _cartService.GetCart());

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("remove-item")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveItemCart(Guid productId)
        {
            if (HasErrorResponse(await _cartService.DeleteItemCart(productId))) return View("Index", await _cartService.GetCart());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {
            var response = await _cartService.ApplyVoucher(voucherCode);

            if (HasErrorResponse(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

    }
}
