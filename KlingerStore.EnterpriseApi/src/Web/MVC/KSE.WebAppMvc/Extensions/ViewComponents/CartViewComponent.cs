using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Extensions.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _cartService.GetCart() ?? new Models.CartViewModel());
        }
    }
}
