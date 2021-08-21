using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using KSE.Order.Domain.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KSE.Order.Domain.Domain
{
    public class Order : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; }
        public Voucher Voucher { get; private set; }

        private readonly List<OrderItem> _OrderItens;
        public IReadOnlyCollection<OrderItem> OrderItens => _OrderItens;

        protected Order() { }

        public Order(Guid clientId, decimal totalValue, List<OrderItem> orderItens,
                     bool voucherUsed = false, decimal discount = 0, Guid? voucherId = null)
        {
            ClientId = clientId;
            VoucherId = voucherId;
            VoucherUsed = voucherUsed;
            Discount = discount;
            TotalValue = totalValue;
            _OrderItens = orderItens;
        }

        public void AuthorizeOrder()
        {
            OrderStatus = OrderStatus.Authorized;
        }
        public void AssignVoucher(Voucher voucher)
        {
            VoucherUsed = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void AssignAddress(ShippingAddress address)
        {
            ShippingAddress = address;
        }
        public void CalculateTotalOrder()
        {
            TotalValue = _OrderItens.Sum(x => x.CalcValue());
            CalculateDiscountAmount();
        }
        public void CalculateDiscountAmount()
        {
            if (!VoucherUsed) return;

            decimal discount = 0;
            var value = TotalValue;

            if (Voucher.TypeDiscountVoucher == TypeDiscountVoucher.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (value * Voucher.Percentage.Value) / 100;
                    value -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    value -= discount;
                }
            }
            TotalValue = value < 0 ? 0 : value;
            Discount = discount;
        }
    }
}
