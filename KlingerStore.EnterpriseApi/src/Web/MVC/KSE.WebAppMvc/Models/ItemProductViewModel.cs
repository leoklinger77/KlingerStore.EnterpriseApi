﻿using System;
using System.Collections.Generic;

namespace KSE.WebAppMvc.Models
{
    public class ItemProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }

    public class CartViewModel
    {
        public decimal TotalValue { get; set; }
        public List<ItemProductViewModel> Itens { get; set; } = new List<ItemProductViewModel>();
    }
}
