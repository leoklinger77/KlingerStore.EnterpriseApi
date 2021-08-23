using System;
using System.Collections.Generic;

namespace KSE.WebAppMvc.Models
{
    public class OrderViewModel
    {
        public int Code { get; set; }
        public int Status { get; set; }
        public DateTime InsertDate { get; set; }
        public decimal TotalValue { get; set; }

        public decimal Discount { get; set; }
        public bool VoucherUsed { get; set; }

        public List<ItemPedidoViewModel> Itens { get; set; } = new List<ItemPedidoViewModel>();


        public class ItemPedidoViewModel
        {
            public Guid ProdutoId { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Value { get; set; }
            public string Image { get; set; }
        }

 

        public AddressViewModel Address { get; set; }

        
    }
}
