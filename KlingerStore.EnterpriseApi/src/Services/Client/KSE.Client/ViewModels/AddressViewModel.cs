using KSE.Core.DomainObjets;
using System;

namespace KSE.Client.ViewModels
{
    public class AddressViewModel : Entity
    {
        public ClientViewModel Client { get; set; }
        public Guid ClientId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
}
