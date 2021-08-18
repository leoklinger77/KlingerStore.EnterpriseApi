using AutoMapper;
using KSE.Client.ViewModels;

namespace KSE.Client.Configuration
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {           
            CreateMap<Models.Address, AddressViewModel>().ReverseMap();

            CreateMap<Models.Client, ClientViewModel>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(x => x.Cpf.Numero))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.Email.EmailAddress));
        }
    }
}
