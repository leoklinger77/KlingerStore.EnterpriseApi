using System;

namespace KSE.Core.Messages.IntegrationEvents.Client
{
    public class RegisteredUserIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public string Ddd { get; private set; }
        public string PhoneNumber { get; private set; }
        public int PhoneType { get; private set; }

        public RegisteredUserIntegrationEvent(Guid id, string name, string email, string cpf, string ddd, string phoneNumber, int phoneType)
        {
            AggregateId = id;

            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
            Ddd = ddd;
            PhoneNumber = phoneNumber;
            PhoneType = phoneType;
        }
    }


}
