using FluentValidation;
using KSE.Core.Messages;
using KSE.Order.Application.DTO;
using System;
using System.Collections.Generic;

namespace KSE.Order.Application.Commands
{
    public class CreatedOrderCommand : Command
    {
        public Guid ClientId { get; set; }
        public decimal TotalValue { get; set; }
        public List<OrderItemDTO> Itens { get; set; } = new List<OrderItemDTO>();

        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientDocument { get; set; }
        public string ClientPhone { get; set; }

        public string VoucherCode { get; set; }
        public bool VoucherUsed { get; set; }
        public decimal Discount { get; set; }

        public ShippingAddressDTO Address { get; set; }

        public string NumberCart { get; set; }
        public string NameCart { get; set; }
        public string ExpirationCArt { get; set; }
        public string CVVCart { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new CreatedOrderCommand.CreatedOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class CreatedOrderCommandValidation : AbstractValidator<CreatedOrderCommand>
        {
            public CreatedOrderCommandValidation()
            {
                RuleFor(x => x.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do client inválido.");

                RuleFor(x => x.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O pedido precisa ter no mínimo 1 item.");

                RuleFor(x => x.TotalValue)
                    .GreaterThan(0)
                    .WithMessage("valo do pedido inválido.");
                RuleFor(x => x.NumberCart)
                    .CreditCard()
                    .WithMessage("Número de cartão inválido.");

                RuleFor(x => x.NameCart)
                    .NotNull()
                    .WithMessage("Nome do portado do cartão requerido.");

                RuleFor(x => x.CVVCart.Length)
                    .GreaterThan(2)
                    .LessThan(5)
                    .WithMessage("O cvv do cartão precisa ter entre 3 ou 4 números.");

                RuleFor(x => x.ExpirationCArt)
                    .NotNull()
                    .WithMessage("Data expiração do cartão requerida.");
            }
        }
    }

    
}
