using FluentValidation;
using FluentValidation.Results;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace KSE.Cart.Models
{
    public class Cart 
    {
        internal const int MAX_QUANTITY_ITEM = 5;
        [BsonId]
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal TotalValue { get; set; }
        public List<CartItem> Itens { get; set; } = new List<CartItem>();        
        
        [JsonIgnore]
        [BsonIgnore]
        public ValidationResult ValidationResult { get; set; }
        public Cart(Guid clientId)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;           
        }
        public Cart() { }

        internal void CalcularTotalCart()
        {
            TotalValue = Itens.Sum(x => x.CalcValue());
        }
        internal bool CartItemExists(CartItem item)
        {
            return Itens.Any(x => x.ProductId == item.ProductId);
        }
        internal CartItem FindByProductId(Guid productId)
        {
            return Itens.FirstOrDefault(x => x.ProductId == productId);
        }        
        internal void AddItem(CartItem item)
        {
            item.AssociarCart(Id);

            if (CartItemExists(item))
            {
                var itemExists = FindByProductId(item.ProductId);
                itemExists.AddUnit(item.Quantity);

                item = itemExists;
                Itens.Remove(itemExists);
            }

            Itens.Add(item);
            CalcularTotalCart();
        }
        internal void UpdateItem(CartItem item)
        {
            item.AssociarCart(Id);            

            Itens.Remove(FindByProductId(item.ProductId));
            Itens.Add(item);            
            CalcularTotalCart();
        }
        internal void UpdateUnits(CartItem item, int quantity)
        {
            item.UpdateUnits(quantity);
            UpdateItem(item);
        }        
        internal void RemoveItem(CartItem item)
        {
            var itemExists = FindByProductId(item.ProductId);

            if (itemExists is null) throw new Exception("O Item não pertence ao pedido");
            Itens.Remove(itemExists);

            CalcularTotalCart();
        }

        internal bool IsValid()
        {
            var erros = Itens.SelectMany(x => new CartItem.CartItemValidation().Validate(x).Errors).ToList();
            erros.AddRange(new Cart.CartValidation().Validate(this).Errors);

            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }
        public class CartValidation : AbstractValidator<Cart>
        {
            public CartValidation()
            {
                RuleFor(x => x.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido.");                
            }
        }
    }    
}
