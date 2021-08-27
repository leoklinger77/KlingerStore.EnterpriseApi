using Grpc.Core;
using KSE.Cart.API.Services.gRPC;
using KSE.Cart.Repository;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KSE.Cart.Services.gRPC
{
    [Authorize]
    public class CartGrpcService : CartPurchase.CartPurchaseBase
    {
        private readonly ILogger<CartGrpcService> _logger;
        private readonly IAspNetUser _aspNetUser;
        private readonly ICartRepository _cartRepository;

        public CartGrpcService(ILogger<CartGrpcService> logger, IAspNetUser aspNetUser, ICartRepository cartRepository)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _cartRepository = cartRepository;
        }

        public override async Task<CartClientResponse> GetCart(GetCartRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Grpc Chamando GetCart");

            var cart = await GetCartClient() ?? new Models.Cart();

            return MapCartToCartProtoResponse(cart);
        }

        private async Task<Models.Cart> GetCartClient()
        {
            return await _cartRepository.GetClient(_aspNetUser.UserId);
        }

        private static CartClientResponse MapCartToCartProtoResponse(Models.Cart cart)
        {
            var cartProto = new CartClientResponse
            {
                Id = cart.Id.ToString(),
                Clientid = cart.ClientId.ToString(),
                Discount = (double)cart.Discount,
                Totalvalue = (double)cart.TotalValue,
                Voucherused = cart.VoucherUsed,
            };

            if(cart.Voucher != null)
            {
                cartProto.Voucher = new VoucherResponse
                {
                    Code = cart.Voucher.Code,
                    Percentage = (double?)cart.Voucher.Percentage ?? 0,
                    DiscountValue = (double?)cart.Voucher.DiscountValue ?? 0,
                    Typediscountvoucher = (int)cart.Voucher.TypeDiscountVoucher
                };                   
            }

            foreach (var item in cart.Itens)
            {
                cartProto.Itens.Add(new CartItemResponse 
                { 
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.image,
                    Productid = item.ProductId.ToString(),
                    Quantity = item.Quantity,
                    Value = (double)item.Value
                });
            }
            return cartProto;
        }
    }
}
