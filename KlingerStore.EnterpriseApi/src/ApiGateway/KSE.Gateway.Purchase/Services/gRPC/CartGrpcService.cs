using KSE.Cart.API.Services.gRPC;
using KSE.Gateway.Purchase.Models.Cart;
using KSE.Gateway.Purchase.Models.Order;
using System;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.gRPC
{
    public interface ICartGrpcService
    {
        Task<CartDTO> GetCart();
    }
    public class CartGrpcService : ICartGrpcService
    {
        private readonly CartPurchase.CartPurchaseClient _cartPurchase;

        public CartGrpcService(CartPurchase.CartPurchaseClient cartPurchase)
        {
            _cartPurchase = cartPurchase;
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _cartPurchase.GetCartAsync(new GetCartRequest());

            return MapCarrinhoClienteProtoResponseToDTO(response);
        }

        private static CartDTO MapCarrinhoClienteProtoResponseToDTO(CartClientResponse carrinhoResponse)
        {
            var carrinhoDTO = new CartDTO
            {
                TotalValue = (decimal)carrinhoResponse.Totalvalue,
                Discount = (decimal)carrinhoResponse.Discount,
                VoucherUsed = carrinhoResponse.Voucherused
            };

            if (carrinhoResponse.Voucher != null)
            {
                carrinhoDTO.Voucher = new VoucherDTO
                {
                    Code = carrinhoResponse.Voucher.Code,
                    Percentage = (decimal?)carrinhoResponse.Voucher.Percentage,
                    DiscountValue = (decimal?)carrinhoResponse.Voucher.DiscountValue,
                    TypeDiscountVoucher = carrinhoResponse.Voucher.Typediscountvoucher
                };
            }

            foreach (var item in carrinhoResponse.Itens)
            {
                carrinhoDTO.Itens.Add(new ItemCartDTO
                {
                    Name = item.Name,
                    Image = item.Image,
                    ProductId = Guid.Parse(item.Productid),
                    Quantity = item.Quantity,
                    Value = (decimal)item.Value
                });
            }

            return carrinhoDTO;
        }
    }
}
