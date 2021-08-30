using FluentValidation;
using KSE.Core.Messages;
using System;

namespace KSE.Client.Application.Commands
{
    public class PhoneCommandHandler : Command
    {
        public Guid ClientId { get; set; }

        public string Ddd { get; private set; }
        public string Number { get; private set; }
        public int PhoneType { get; private set; }

        public PhoneCommandHandler(Guid clientId, string ddd, string number, int phoneType)
        {
            ClientId = clientId;
            Ddd = ddd;
            Number = number;
            PhoneType = phoneType;
        }

        public override bool IsValid()
        {
            ValidationResult = new PhoneCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class PhoneCommandValidation : AbstractValidator<PhoneCommandHandler>
        {
            public PhoneCommandValidation()
            {
                RuleFor(x => x.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("ClientId inválido.");

                RuleFor(x => x.Ddd)
                    .NotEmpty()
                    .WithMessage("Ddd obrigatorio.");

                RuleFor(x => x.Number)
                    .NotEmpty()
                    .WithMessage("Número obrigatorio.");
            }
        }
    }
}
