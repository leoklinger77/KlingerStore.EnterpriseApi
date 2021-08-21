using KSE.Core.DomainObjets;
using System;

namespace KSE.Order.Domain.Domain
{
    public class ShippingAddress : Entity
    {
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        protected ShippingAddress() { }
        public ShippingAddress(string street, string number, string complement, string district, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            ZipCode = zipCode.Replace("-", "");
            City = city;
            State = state;           
        }
    }
}
