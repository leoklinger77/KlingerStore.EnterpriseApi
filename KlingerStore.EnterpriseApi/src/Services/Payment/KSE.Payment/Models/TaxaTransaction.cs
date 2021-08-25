using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;

namespace KSE.Payment.Models
{
    public class TaxaTransaction : Entity, IAggregateRoot
    {
        public int Installments { get; private set; }
        public double Taxa { get; private set; }
        public bool Active { get; set; }

        protected TaxaTransaction() { }

        public TaxaTransaction(int installments, double taxa)
        {
            Installments = installments;
            Taxa = taxa;
            Active = true;
        }
    }
}
