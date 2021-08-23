using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using KSE.Payment.Models.Enums;
using System;
using System.Collections.Generic;

namespace KSE.Payment.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment()
        {
            Transaction = new List<Transaction>();
        }

        public Guid OrderId { get; set; }
        public TypePayment TypePayment { get; set; }
        public decimal Value { get; set; }
        public CreditCart CreditCart { get; set; }
        
        public ICollection<Transaction> Transaction { get; set; }

        public void CreatedTransation(Transaction transacao)
        {
            Transaction.Add(transacao);
        }

        public void AdicionarTransacao(Transaction transacao)
        {
            Transaction.Add(transacao);
        }
    }
}