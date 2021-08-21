using FluentValidation;
using KSE.Core.Messages;
using System;

namespace KSE.Client.Application.Commands
{
    public class AddressCommandHandler : Command
    {
        public Guid ClientId { get; set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        public AddressCommandHandler() { }
        public AddressCommandHandler(string street, string number, string complement, string district, string zipCode, string city, string state)
        {            
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddressCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddressCommandValidation : AbstractValidator<AddressCommandHandler>
        {
            public AddressCommandValidation()
            {
                RuleFor(x => x.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("ClientId inválido.");

                RuleFor(x => x.Street)
                    .NotEmpty()
                    .WithMessage("Logradouro inválido.");

                RuleFor(x => x.Number)
                    .NotEmpty()
                    .WithMessage("Número inválido.");                

                RuleFor(x => x.District)
                    .NotEmpty()
                    .WithMessage("Bairro inválido.");

                RuleFor(x => x.ZipCode)
                    .NotEmpty()
                    .WithMessage("Cep inválido.")
                    .Length(8,8).WithMessage("Cep deve ter 8 caracteres.");

                RuleFor(x => x.City)
                    .NotEmpty()
                    .WithMessage("Cidade inválido.");

                RuleFor(x => x.State)
                    .NotEmpty()
                    .WithMessage("Estado inválido.");
            }
        }
    }
}
