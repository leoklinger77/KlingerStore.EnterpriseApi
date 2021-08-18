using AutoMapper;
using KSE.Client.ViewModels;

namespace KSE.Client.Configuration
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Models.Client, ClientViewModel>().ReverseMap();
            CreateMap<Models.Address, AddressViewModel>().ReverseMap();
        }
    }
}
