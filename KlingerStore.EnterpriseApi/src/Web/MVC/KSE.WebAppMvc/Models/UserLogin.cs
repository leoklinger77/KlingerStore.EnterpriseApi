using System.ComponentModel.DataAnnotations;

namespace KSE.WebAppMvc.Models
{
    public class UserLogin
    {
        
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class UserLoginWith2fa
    {
        
        public string Email { get; set; }
        [Required]
        [StringLength(7, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Código autenticador")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Lembre-se desta máquina")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
    public class LoginWithRecovery
    {        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Código de recuperação")]
        public string RecoveryCode { get; set; }
        public string Email { get; set; }
    }
}
