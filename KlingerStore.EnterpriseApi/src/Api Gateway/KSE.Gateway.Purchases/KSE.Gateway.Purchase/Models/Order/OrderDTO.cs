using KSE.Gateway.Purchase.Models.Cart;
using KSE.Gateway.Purchase.Models.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KSE.Gateway.Purchase.Models.Order
{
    public class OrderDTO
    {
        public int Code { get; set; }        
        public int Status { get; set; }
        public DateTime InsertDate { get; set; }
        public decimal TotalValue { get; set; }

        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherUsed { get; set; }

        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientDocument { get; set; }
        public string ClientPhone { get; set; }

        public List<ItemCartDTO> Itens { get; set; } = new List<ItemCartDTO>();    

        public AddressDTO Address { get; set; }        

        [Required(ErrorMessage = "Informe o número do cartão")]
        [DisplayName("Número do Cartão")]
        public string NumberCart { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string NameCart { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]        
        [Required(ErrorMessage = "Informe o vencimento")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string ExpirationCart { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [DisplayName("Código de Segurança")]
        public string CvvCart { get; set; }       

    }
}
