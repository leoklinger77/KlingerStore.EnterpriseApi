using KSE.Client.Models.Enum;
using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using System;

namespace KSE.Client.Models
{
    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Cpf Cpf { get; private set; }
        public Email Email { get; private set; }        
        public ClientStatus Status { get; private set; }
        public Address Address { get; private set; }

        protected Client() { }
        public Client(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Status = ClientStatus.Active;

            IsValid();
        }
        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }
        public void AddressAtribrutes(Address address)
        {
            Address = address;
        }

        public override bool IsValid()
        {
            //Todo Regras CLient
            return true;
        }
    }
}
