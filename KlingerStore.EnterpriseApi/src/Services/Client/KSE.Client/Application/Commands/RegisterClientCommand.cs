using FluentValidation;
using KSE.Core.Messages;
using KSE.Core.Tools;
using System;

namespace KSE.Client.Application.Commands
{
    public class RegisterClientCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }

        public RegisterClientCommand(Guid id, string name, string cpf, string email)
        {
            AggregateId = id;

            Id = id;
            Name = name;
            Cpf = cpf;
            Email = email;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterClientValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RegisterClientValidation : AbstractValidator<RegisterClientCommand>
        {
            public RegisterClientValidation()
            {
                RuleFor(x => x.Id)
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
