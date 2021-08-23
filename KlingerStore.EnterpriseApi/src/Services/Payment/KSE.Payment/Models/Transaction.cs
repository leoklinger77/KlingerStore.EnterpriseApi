using KSE.Core.DomainObjets;
using KSE.Payment.Models.Enums;
using System;


namespace KSE.Payment.Models
{
    public class Transaction : Entity
    {
        public string CodeAuthorization { get; set; }
        public string BrandCart { get; set; }        
        public decimal TotalValue { get; set; }
        public decimal CostTransaction { get; set; }
        public StatusTransaction Status { get; set; }
        public DateTime DateTransaction { get; set; }
        public string TID { get; set; } // Id
        public string NSU { get; set; } // Meio (paypal)

        public Guid PaymentId { get; set; }

        // EF Relation
        public Payment Payment { get; set; }
    }
}