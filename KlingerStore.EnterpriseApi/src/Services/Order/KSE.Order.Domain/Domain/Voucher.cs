
using FluentValidation;
using FluentValidation.Results;
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
        
        public ValidationResult ValidationResult;

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
        public void DebitBalance()
        {            
            Quantity = -1;
            if (Quantity >= 1) return;
            AlreadyUsed();
        }

        public override bool IsValid()
        {
            ValidationResult = new VoucherValidate().Validate(this);
            return ValidationResult.IsValid;
        }

        

        public class VoucherValidate : AbstractValidator<Voucher>
        {
            public VoucherValidate()
            {
                RuleFor(x => x.SelfLiveValid())
                    .NotEqual(true)
                    .WithMessage("Este voucher ja foi expirado.");

                RuleFor(x => x.VoucherQuantityValid())
                    .NotEqual(true)
                    .WithMessage("Este voucher já foi utilizado.");

                RuleFor(x => x.VoucherValid())
                    .NotEqual(true)
                    .WithMessage("Este voucher não está mais ativo.");
            }
        }
    }
}
