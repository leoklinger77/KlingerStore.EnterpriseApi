using KSE.Core.Interfaces;
using KSE.Order.Domain.Domain;
using KSE.Order.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KSE.Order.Infrastructure.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly OrderContext _orderContext;

        public VoucherRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public IUnitOfWork UnitOfWork => _orderContext;

        public async Task<Voucher> GetVoucherPerCode(string code)
        {
            return await _orderContext.Voucher.FirstOrDefaultAsync(x => x.Code == code);
        }
        public void Update(Voucher voucher)
        {
            _orderContext.Update(voucher);
        }

        public void Dispose()
        {
            _orderContext?.DisposeAsync();
        }        
    }
}
