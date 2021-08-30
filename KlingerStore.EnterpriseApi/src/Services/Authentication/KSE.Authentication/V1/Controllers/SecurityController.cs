using KSE.Authentication.Models;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KSE.Authentication.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:ApiVersion}/security")]
    public class SecurityController : MainController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public SecurityController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        
        [HttpPost("my-email")]
        public async Task<IActionResult> ChangeEmail(Email updateEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return CustomResponse();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (updateEmail.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, updateEmail.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = updateEmail.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    updateEmail.NewEmail,
                    "Confirme seu email",
                    $"Por favor, confirme sua conta até <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicando aqui.</a>.");

                return CustomResponse("Link de confirmação para alteração de e-mail enviado. Por favor verifique seu email.");
            }

            return CustomResponse("Seu e-mail não foi alterado.");
        }

        [HttpPost("my-email-verification")]
        public async Task<IActionResult> SendVerificationEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return CustomResponse(await LoadAsync(user));
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirmar seu e-mail",
                $"Por favor, confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicando aqui.</a>.");

            return CustomResponse("E-mail de verificação enviado. Por favor verifique seu email.");
        }

        private async Task<Email> LoadAsync(IdentityUser user)
        {
            return new Email
            {
                EmailAddress = await _userManager.GetEmailAsync(user),
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user)
            };
        }
    }
}
