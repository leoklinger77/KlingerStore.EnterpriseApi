using KSE.Client.Models.Enum;
using KSE.Core.DomainObjets;
using KSE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KSE.Client.Models
{
    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Cpf Cpf { get; private set; }
        public Email Email { get; private set; }        
        public ClientStatus Status { get; private set; }
        public Address Address { get; private set; }
        public List<Phone> Phones { get; set; } = new List<Phone>();

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

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }
        public void AddressAtribrutes(Address address)
        {
            Address = address;
        }

        public void AddPhone(Phone phone)
        {
            Phones.Add(phone);
        }

        public void ChangeCel(string ddd, string celular)
        {
            var cel = Phones.Where(x => x.PhoneType == PhoneType.Cell).FirstOrDefault();

            if(string.IsNullOrEmpty(ddd) && string.IsNullOrEmpty(celular))
            {
                throw new DomainException("ddd e celular inválido.");
            }

            cel.ChangeDdd(ddd);
            cel.ChangeNumber(celular);
        }

        public override bool IsValid()
        {
            //Todo Regras CLient
            return true;
        }
    }
}
