using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using KSE.Order.Domain.Domain.Enumerations;
using System;

namespace KSE.Order.Domain.Domain
{
    public class Voucher : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Quantity { get; private set; }
        public TypeDiscountVoucher TypeDiscountVoucher { get; private set; }
        public DateTime ShelfLife { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        protected Voucher() { }

        public void AlreadyUsed()
        {
            Active = false;
            Used = true;
            Quantity = 0;
        }

        public bool ValidForUse()
        {
            return SelfLiveValid() && VoucherQuantityValid() && VoucherValid();
        }

        public bool SelfLiveValid()
        {
            return ShelfLife >= DateTime.Now;
        }
        public bool VoucherQuantityValid()
        {
            return Quantity > 0;
        }
        public bool VoucherValid()
        {
            return Active;
        }
    }
}
