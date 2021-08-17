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
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _cartService.GetCart());
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemCart(ItemProductViewModel item)
        {
            var product = await _catalogService.FindById(item.ProductId);

            ValidationItemCart(product, item.Quantity);
            if (!OperationValid()) return View("Index", await _cartService.GetCart());

            item.Name = product.Name;
            item.Value = product.Value;
            item.Image = product.Image;            

            if (HasErrorResponse(await _cartService.AddItemCart(item))) return View("Index", await _cartService.GetCart());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            ValidationItemCart(await _catalogService.FindById(productId), quantity);
            if (!OperationValid()) return View("Index", await _cartService.GetCart());

            if (HasErrorResponse(await _cartService.UpdateItemCart(productId,
                new ItemProductViewModel
                {
                    ProductId = productId,                    
                    Quantity = quantity                    
                }))) return View("Index", await _cartService.GetCart());

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("remove-item")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveItemCart(Guid productId)
        {
            if (await _catalogService.FindById(productId) is null)
            {
                AddError("Produto inesxistente!");
                return View("Index", await _cartService.GetCart());
            }

            if (HasErrorResponse(await _cartService.RemoveItemCart(productId))) return View("Index", await _cartService.GetCart());

            return RedirectToAction(nameof(Index));
        }

        private void ValidationItemCart(ProductViewModel product, int quantity)
        {
            if (product is null) AddError("Produto inesxistente!");
            if (quantity < 1) AddError("");
            if (quantity > product.QuantityStock) AddError($"O producto {product.Name} possui {product.QuantityStock} quantidades em estoque!" );
        }
    }
}
