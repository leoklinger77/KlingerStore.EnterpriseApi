using AutoMapper;
using KSE.Order.Application.DTO;
using KSE.Order.Domain.Domain;

namespace KSE.Order.Configuration
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Voucher, VoucherDTO>().ReverseMap();

            CreateMap<Domain.Domain.Order, OrderDTO>()
                .ForMember(dest => dest.VoucherCode, opt => opt.MapFrom(x => x.Voucher.Code));                
            
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
        }
    }
}
