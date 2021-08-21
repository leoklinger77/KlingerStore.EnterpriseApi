using System;

namespace KSE.Order.Application.DTO
{
    public class ShippingAddressDTO
    {
        public Guid OrderId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
