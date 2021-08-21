using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    [Authorize]
    [Route("Order")]
    public class OrderController : MainController
    {
        private readonly IClientService _clientService;
        private readonly IGatewayPurchaseService _gatewayPurchaseService;

        public OrderController(IClientService clientService, IGatewayPurchaseService gatewayPurchaseService)
        {
            _clientService = clientService;
            _gatewayPurchaseService = gatewayPurchaseService;
        }

        [HttpGet("ShippingAddress")]
        public async Task<IActionResult> ShippingAddress()
        {
            var cart = await _gatewayPurchaseService.GetCart();
            if (cart.Itens.Count == 0) return RedirectToAction("Index", "Cart");

            var address = await _clientService.GetAddress();            

            return View(MappingOrderAddress(cart, address));
        }

        [HttpGet("payment")]
        public async Task<IActionResult> Payment()
        {
            var cart = await _gatewayPurchaseService.GetCart();
            if (cart.Itens.Count == 0) return RedirectToAction("Index", "Cart");

            var address = await _clientService.GetAddress();

            return View(MappingOrderAddress(cart, address));
        }

        [HttpPost("FinishOrder")]
        public async Task<IActionResult> FinishOrder(TransactionOrderViewModel transactionOrder)
        {
            if (!ModelState.IsValid) return View("Payment", MappingOrderAddress(await _gatewayPurchaseService.GetCart()));

            var response = await _gatewayPurchaseService.FinishOrder(transactionOrder);

            if (HasErrorResponse(response))
            {
                var cart = await _gatewayPurchaseService.GetCart();
                if (cart.Itens.Count == 0) return RedirectToAction("Index", "Cart");

                return View("Payment", MappingOrderAddress(cart));
            }

            return RedirectToAction("OrderCompleted");
        }

        [HttpGet("OrderCompleted")]
        public async Task<IActionResult> OrderCompleted()
        {
            return View("Confirmar", await _gatewayPurchaseService.GetListOrder());
        }

        private TransactionOrderViewModel MappingOrderAddress(CartViewModel cart, AddressViewModel address = null)
        {
            return new TransactionOrderViewModel
            {
                Address = address,
                Discount = cart.Discount,
                TotalValue = cart.TotalValue,
                VoucherCode = cart.Voucher?.Code,
                VoucherUsed = cart.VoucherUsed,
                Itens = cart.Itens
            };
        }
    }
}
