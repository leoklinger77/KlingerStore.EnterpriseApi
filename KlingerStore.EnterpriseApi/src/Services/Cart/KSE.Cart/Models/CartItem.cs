using FluentValidation;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace KSE.Cart.Models
{
    public class CartItem
    {
        public CartItem()
        {
            Id = Guid.NewGuid();
        }
        [BsonId]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public string image { get; set; }
        public Guid CartId { get; set; }

        internal void AssociarCart(Guid id)
        {
            CartId = id;
        }
        internal decimal CalcValue()
        {
            return Quantity * Value;
        }
        internal void AddUnit(int quantity)
        {
            Quantity += quantity;
        }
        internal void UpdateUnits(int quantity)
        {
            Quantity = quantity;
        }
        internal bool IsValid()
        {
            return new CartItemValidation().Validate(this).IsValid;
        }

        public class CartItemValidation : AbstractValidator<CartItem>
        {
            public CartItemValidation()
            {
                RuleFor(x => x.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido.");

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("O nome do produto não foi informado.");

                RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage(item => $"A quantidade miníma para o {item.Name} é 1.");

                RuleFor(x => x.Quantity)
                    .LessThanOrEqualTo(Cart.MAX_QUANTITY_ITEM)
                    .WithMessage(item => $"A quantidade máxima do {item.Name} é {Cart.MAX_QUANTITY_ITEM}.");

                RuleFor(x => x.Value)
                    .GreaterThan(0)
                    .WithMessage(item => $"O valor do {item.Name} preicsa ser maior que 0.");
            }
        }

    }
}
