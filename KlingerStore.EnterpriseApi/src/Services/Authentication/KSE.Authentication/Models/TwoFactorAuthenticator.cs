using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KSE.Authentication.Models
{
    public class TwoFactorAuthenticator
    {
        [Required]
        [StringLength(7, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Código de verificação")]
        public string Code { get; set; }
        public LoadSharedKeyAndQrCodeUri LoadSharedKeyAndQrCodeUri { get; set; }
        public string StatusMessage { get; set; }
        public string[] RecoveryCodes { get; set; }

        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        [BindProperty]
        public bool Is2faEnabled { get; set; }

        public bool IsMachineRemembered { get; set; }
    }

    public class LoadSharedKeyAndQrCodeUri
    {
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }
    }
}
