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
        public int Code { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public ShippingAddress Address { get; private set; }
        public Voucher Voucher { get; private set; }

        private readonly List<OrderItem> _Itens;
        public IReadOnlyCollection<OrderItem> Itens => _Itens;

        protected Order() { }

        public Order(Guid clientId, decimal totalValue, List<OrderItem> orderItens,
                     bool voucherUsed = false, decimal discount = 0, Guid? voucherId = null)
        {
            ClientId = clientId;
            VoucherId = voucherId;
            VoucherUsed = voucherUsed;
            Discount = discount;
            TotalValue = totalValue;
            _Itens = orderItens;
        }

        public void AuthorizeOrder()
        {
            OrderStatus = OrderStatus.Authorized;
        }

        public void FinishOrder()
        {
            OrderStatus = OrderStatus.PaidOut;
        }
        public void CanceledOrder()
        {
            OrderStatus = OrderStatus.canceled;
        }

        public void AssignVoucher(Voucher voucher)
        {
            VoucherUsed = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void AssignAddress(ShippingAddress address)
        {
            Address = address;
        }
        public void CalculateTotalOrder()
        {
            TotalValue = _Itens.Sum(x => x.CalcValue());
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
