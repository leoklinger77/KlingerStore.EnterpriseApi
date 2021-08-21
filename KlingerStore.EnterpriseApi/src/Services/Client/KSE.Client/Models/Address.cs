using KSE.Core.DomainObjets;
using System;

namespace KSE.Client.Models
{
    public class Address : Entity
    {
        public Client Client { get; set; }
        public Guid ClientId { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        protected Address() { }
        public Address(string street, string number, string complement, string district, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            ZipCode = zipCode.Replace("-","");
            City = city;
            State = state;

            IsValid();
        }

        public void AddCliente(Guid clientId)
        {
            if (clientId == Guid.Empty) return;

            ClientId = clientId;
        }

        public override bool IsValid()
        {
            //Todo Regras Address
            return true;
        }
    }
}
