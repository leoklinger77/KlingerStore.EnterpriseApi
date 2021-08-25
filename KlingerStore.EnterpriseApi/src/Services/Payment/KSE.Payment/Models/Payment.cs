using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using KSE.Payment.KlingerPag.Models;
using KSE.Payment.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KSE.Payment.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment()
        {
            Transaction = new List<Transaction>();
        }

        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public TypePayment TypePayment { get; set; }
        public decimal Value { get; set; }
        [NotMapped]
        public string ClientName { get; set; }
        [NotMapped]
        public string ClientDocument { get; set; }
        [NotMapped]
        public string ClientEmail { get; set; }
        [NotMapped]
        public string ClientPhone { get; set; }
        [NotMapped]
        public string State { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string Neighborhood { get; set; }
        [NotMapped]
        public string Street { get; set; }
        [NotMapped]
        public string Number { get; set; }
        [NotMapped]
        public string Zipcode { get; set; }

        [NotMapped]
        public List<ItemViewModel> Itens { get; set; }


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