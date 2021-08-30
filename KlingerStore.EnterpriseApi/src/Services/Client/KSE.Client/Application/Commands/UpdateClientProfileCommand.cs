using FluentValidation;
using KSE.Core.Messages;
using KSE.Core.Tools;
using System;
using System.Collections.Generic;

namespace KSE.Client.Application.Commands
{
    public class UpdateClientProfileCommand : Command
    {
        public Guid ClientId { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }

        public List<PhoneCommandHandler> Phones { get; private set; }


        public UpdateClientProfileCommand(Guid id, string name, string cpf, string email, List<PhoneCommandHandler> phones)
        {
            AggregateId = id;

            ClientId = id;
            Name = name;
            Cpf = cpf;
            Email = email;
            Phones = phones;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateClientProfileValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UpdateClientProfileValidation : AbstractValidator<UpdateClientProfileCommand>
        {
            public UpdateClientProfileValidation()
            {
                RuleFor(x => x.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido.");

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("O nome do cliente não foi informado.");

                RuleFor(x => x.Cpf)
                    .Must(ValidationMethods.IsCpf)
                    .WithMessage("O Cpf informado não é válido.");

                RuleFor(x => x.Email)
                    .Must(ValidationMethods.IsEmail)
                    .WithMessage("O e-mail informado não é válido.");
            }
        }
    }
}
