using System.ComponentModel.DataAnnotations;

namespace KSE.Authentication.Models
{
    public class Email
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Novo e-email")]
        public string NewEmail { get; set; }

        public string EmailAddress { get; set; }

        public bool IsEmailConfirmed { get; set; }        
    }
}
