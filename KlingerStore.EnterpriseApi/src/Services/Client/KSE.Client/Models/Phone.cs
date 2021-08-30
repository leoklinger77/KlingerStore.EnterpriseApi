using KSE.Client.Models.Enum;
using KSE.Core.DomainObjets;
using System;

namespace KSE.Client.Models
{
    public class Phone : Entity
    {
        public string Ddd { get; private set; }
        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public Guid ClientId { get; private set; }
        public Client Client { get; private set; }

        protected Phone() { }

        public Phone(string ddd, string number, PhoneType phoneType)
        {            
            Number = number;
            PhoneType = phoneType;
            Ddd = ddd;
        }

        public void AddCliente(Guid clientId)
        {
            if (clientId == Guid.Empty) return;

            ClientId = clientId;
        }

        internal void ChangeDdd(string ddd)
        {
            Ddd = ddd;
        }

        internal void ChangeNumber(string number)
        {
            Number = number;
        }
    }
}
