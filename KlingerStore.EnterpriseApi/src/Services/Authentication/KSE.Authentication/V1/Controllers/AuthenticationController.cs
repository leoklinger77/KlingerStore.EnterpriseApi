using KSE.Authentication.Extensions;
using KSE.Authentication.Models;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Core.Messages.IntegrationEvents.Client;
using KSE.MessageBus;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KSE.Authentication.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:ApiVersion}/Authentication")]
    public class AuthenticationController : MainController
    {
        private readonly IMessageBus _bus;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GeneretorToken _token;
        private readonly IEmailSender _emailSender;

        public AuthenticationController(SignInManager<IdentityUser> signInManager,
                                        UserManager<IdentityUser> userManager,
                                        GeneretorToken token,
                                        IMessageBus bus,
                                        IEmailSender emailSender)
        {
            _bus = bus;
            _signInManager = signInManager;
            _userManager = userManager;
            _token = token;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                var clientResult = await RegisterClient(userRegister);

                if (!clientResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clientResult.ValidationResult);
                }
                Url.Content("~/");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = new Uri($"https://localhost:44387/email-verification?userId={user.Id}&code={code}").ToString();

                await _emailSender.SendEmailAsync(userRegister.Email, "Confirmar seu e-mail",
                    $"Por favor, confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                return CustomResponse();
            }
            foreach (var item in result.Errors)
            {
                AddErros(item.Description);
            }
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("FirstAccess")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);

            if (result.RequiresTwoFactor)
            {
                AddErros("2FA Requerido.");
            }
            else if (result.Succeeded)
            {
                return CustomResponse(await _token.TokenJwt(userLogin.Email));
            }
            else if (result.IsLockedOut)
            {
                AddErros("Usuario bloqueado por tentativas inválidas.");
            }
            else
            {
                AddErros("Usuario ou senha inválidos.");
            }
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("FirstAccess-with2fa")]
        public async Task<IActionResult> LoginWith2fa(UserLoginWith2fa userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                AddErros($"Não foi possível carregar o usuário de autenticação de dois fatores.");
                return CustomResponse();
            }

            var authenticatorCode = userLogin.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, userLogin.RememberMe, userLogin.RememberMachine);

            if (result.Succeeded)
            {
                return CustomResponse(await _token.TokenJwt(userLogin.Email));
            }
            else if (result.IsLockedOut)
            {
                AddErros("Usuario bloqueado por tentativas inválidas.");
            }
            else
            {
                AddErros("Usuario ou senha inválidos.");
            }
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("FirstAccess-withRecoveryCode")]
        public async Task<IActionResult> LoginWithRecoveryCode(UserLoginWithRecoveryCode userLogin)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                AddErros($"Não foi possível carregar o usuário de autenticação de dois fatores.");
                return CustomResponse();
            }

            var recoveryCode = userLogin.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                return CustomResponse(await _token.TokenJwt(userLogin.Email));
            }
            if (result.IsLockedOut)
            {
                AddErros("Usuario bloqueado por tentativas inválidas.");
            }
            else
            {
                AddErros("Código de recuperação inválido inserido.");
            }
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken) && refreshToken != Guid.Empty.ToString())
            {
                AddErros("Refresh Token inválido");
                return CustomResponse();
            }

            var token = await _token.GetRefreshToken(Guid.Parse(refreshToken));
            if (token is null)
            {
                AddErros("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _token.TokenJwt(token.UserName));
        }       

        [AllowAnonymous]
        [HttpGet("email-confirmed")]
        public async Task<IActionResult> EmailConfirmed([FromQuery] string userId, [FromQuery] string code)
        {
            if (userId == null || code == null)
            {
                AddErros("User não encontrado.");
                return CustomResponse();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {                
                AddErros($"Não foi possível carregar o usuário com ID '{userId}'.");
                return CustomResponse();
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {                
                return CustomResponse(await _token.TokenJwt(user.Email));
            }

            AddErros("Erro ao confirmar seu e-mail.");
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegisterClient(UserRegister userRegister)
        {
            var user = await _userManager.FindByEmailAsync(userRegister.Email);
            try
            {
                return await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(
                new RegisteredUserIntegrationEvent(Guid.Parse(user.Id),
                                                    userRegister.FirstName + " " + userRegister.LastName,
                                                    userRegister.Email,
                                                    userRegister.Cpf,
                                                    userRegister.NumberPhone.Remove(2),
                                                    userRegister.NumberPhone.Remove(0, 2),
                                                    userRegister.PhoneType));
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }

        }
    }
}
