using KSE.Core.Messages;
using System;

namespace KSE.Client.Application.Events
{
    public class RegisteredCustomerEvent : Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }

        public RegisteredCustomerEvent(Guid id, string name, string cpf, string email)
        {
            Id = id;
            Name = name;
            Cpf = cpf;
            Email = email;
        }
    }
}
