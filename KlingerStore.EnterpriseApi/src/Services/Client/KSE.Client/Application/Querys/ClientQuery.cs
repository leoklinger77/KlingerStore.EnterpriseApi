using AutoMapper;
using KSE.Client.Models.Interfaces;
using KSE.Client.ViewModels;
using System;
using System.Threading.Tasks;

namespace KSE.Client.Application.Querys
{
    public class ClientQuery : IClientQuery
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientQuery(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<AddressViewModel> GetAddress(Guid id)
        {
            var address = await _clientRepository.GetAddress(id);

            if (address is null) return null;

            return _mapper.Map<AddressViewModel>(address);
        }

        public async Task<ClientViewModel> GetClient(Guid id)
        {
            var client = await _clientRepository.GetClient(id);

            if (client is null) return null;

            return _mapper.Map<ClientViewModel>(client);
        }
    }
}
