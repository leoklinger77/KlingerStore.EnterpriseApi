using AutoMapper;
using KSE.Order.Application.DTO;
using KSE.Order.Domain.Interfaces;
using System.Threading.Tasks;

namespace KSE.Order.Application.Querys
{
    public class VoucherQuery : IVoucherQuery
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;
        public VoucherQuery(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<VoucherDTO> GetVoucherPerCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherPerCode(code);

            if (voucher is null) return null;

            if (voucher.ValidForUse()) return null;

            return _mapper.Map<VoucherDTO>(voucher);
        }
    }
}
