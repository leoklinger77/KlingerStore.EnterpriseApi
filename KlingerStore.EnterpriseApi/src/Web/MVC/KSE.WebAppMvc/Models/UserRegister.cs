using KSE.WebAppMvc.Extensions.DataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace KSE.WebAppMvc.Models
{
    public class UserRegister
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(254, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        [Cpf]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage ="O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6 )]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        public string NumberPhone { get; set; }        
        public int PhoneType { get; set; }
    }
}
