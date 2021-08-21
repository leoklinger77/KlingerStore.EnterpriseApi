using System.ComponentModel.DataAnnotations;

namespace KSE.WebAppMvc.Models
{
    public class UserUpdatePassword
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
