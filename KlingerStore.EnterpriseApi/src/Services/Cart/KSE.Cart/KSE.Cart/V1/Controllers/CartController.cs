using KSE.Cart.Models;
using KSE.Cart.Repository;
using KSE.WebApi.Core.Controllers;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Cart.V1.Controllers
{
    [Authorize]
    [Route("V1/Cart")]
    public class CartController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly ICartRepository _cartRepository;

        public CartController(IAspNetUser user, ICartRepository cartRepository)
        {
            _user = user;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<Models.Cart> GetCart()
        {
            return await FindCartClient() ?? new Models.Cart();
        }

        [HttpPost]
        public async Task<IActionResult> AddItemCart([FromBody] CartItem item)
        {
            Models.Cart cart = await FindCartClient();

            if (cart == null)
                cart = await NewCart(item);
            else
                await CartExist(cart, item);

            CartIsValid(cart);
            if (!OperationVation()) return CustomResponse();
            return CustomResponse();
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateITemCart(Guid productId, CartItem item)
        {
            var cart = await FindCartClient();
            var itemCarrinho = await FindCartItemValid(productId, cart, item);
            if (itemCarrinho == null) return CustomResponse();

            cart.UpdateUnits(itemCarrinho, item.Quantity);

            CartIsValid(cart);
            if (!OperationVation()) return CustomResponse();
            await _cartRepository.Update(cart.Id, cart);
            return CustomResponse();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItemCart(Guid productId)
        {
            var cart = await FindCartClient();
            var itemCarrinho = await FindCartItemValid(productId, cart);
            if (itemCarrinho == null) return CustomResponse();

            cart.RemoveItem(itemCarrinho);

            CartIsValid(cart);
            if (!OperationVation()) return CustomResponse();
            await _cartRepository.Update(cart.Id, cart);
            return CustomResponse();
        }

        [HttpPost("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(Voucher voucher)
        {
            var cart = await FindCartClient();
            cart.ApplyVoucher(voucher);
            await _cartRepository.Update(cart.Id, cart);

            return CustomResponse();

        }

        private async Task<Models.Cart> FindCartClient()
        {
            return await _cartRepository.GetClient(_user.UserId);
        }

        private async Task<Models.Cart> NewCart(CartItem item)
        {
            var cart = new Models.Cart(_user.UserId);
            cart.AddItem(item);

            return await _cartRepository.Create(cart);
        }

        private async Task CartExist(Models.Cart cart, CartItem item)
        {
            var cartDb = cart.CartItemExists(item);

            cart.AddItem(item);

            if (cartDb)
            {
                cart.FindByProductId(item.ProductId);
            }
            await _cartRepository.Update(cart.Id, cart);
        }

        private async Task<CartItem> FindCartItemValid(Guid produtoId, Models.Cart carrinho, CartItem item = null)
        {
            if (item != null && produtoId != item.ProductId)
            {
                AddErros("O item não corresponde ao informado");
                return null;
            }

            if (carrinho == null)
            {
                AddErros("Carrinho não encontrado");
                return null;
            }

            var itemCarrinho = carrinho.Itens
                .FirstOrDefault(i => i.CartId == carrinho.Id && i.ProductId == produtoId);

            if (itemCarrinho == null || !carrinho.CartItemExists(itemCarrinho))
            {
                AddErros("O item não está no carrinho");
                return null;
            }

            return itemCarrinho;
        }

        private bool CartIsValid(Models.Cart carrinho)
        {
            if (carrinho.IsValid()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(e => AddErros(e.ErrorMessage));
            return false;
        }
    }
}
