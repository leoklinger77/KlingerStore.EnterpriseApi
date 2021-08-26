using KSE.WebAppMvc.Extensions.DataAnnotation;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KSE.WebAppMvc.Models
{
    public class TransactionOrderViewModel
    {
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherUsed { get; set; }

        public List<CartItemViewModel> Itens { get; set; } = new List<CartItemViewModel>();

        public AddressViewModel Address { get; set; }

        [Required(ErrorMessage = "Informe o número do cartão")]
        [StringLength(16, ErrorMessage = "O número do cartão deve conter 16 digitos", MinimumLength = 16)]
        [DisplayName("Número do Cartão")]
        public string NumberCart { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string NameCart { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]
        [ExpirationCart]
        [Required(ErrorMessage = "Informe o vencimento")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string ExpirationCart { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [StringLength(4, ErrorMessage = "O número do cvv deve ter entre {2} e {1} digitos", MinimumLength = 3)]
        [DisplayName("Código de Segurança")]
        public string CvvCart { get; set; }

        public int Installments { get; set; }
    }
}
