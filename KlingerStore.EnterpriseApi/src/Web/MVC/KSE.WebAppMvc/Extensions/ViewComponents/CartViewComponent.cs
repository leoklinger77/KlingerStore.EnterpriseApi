using KSE.WebAppMvc.Extensions.Exceptions;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Extensions.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IGatewayPurchaseService _cartService;

        public CartViewComponent(IGatewayPurchaseService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {            
            try
            {                 
                return View(await _cartService.GetQuantityCart());
            }
            catch (CustomHttpRequestException)
            {
                return View(0);
            }
        }
    }
}
