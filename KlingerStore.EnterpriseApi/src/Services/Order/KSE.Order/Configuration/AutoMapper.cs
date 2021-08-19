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
        }
    }
}
