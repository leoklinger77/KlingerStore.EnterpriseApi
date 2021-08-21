using KSE.Authentication.Extensions;
using KSE.Authentication.Models;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Core.Messages.IntegrationEvents.Client;
using KSE.MessageBus;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KSE.Authentication.V1.Controllers
{
    [Authorize]
    [Route("V1/Authentication")]
    public class AuthenticationController : MainController
    {
        private readonly IMessageBus _bus;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GeneretorToken _token;
        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public AuthenticationController(SignInManager<IdentityUser> signInManager,
                                        UserManager<IdentityUser> userManager,
                                        GeneretorToken token,
                                        IMessageBus bus,
                                        UrlEncoder urlEncoder)
        {
            _bus = bus;
            _signInManager = signInManager;
            _userManager = userManager;
            _token = token;
            _urlEncoder = urlEncoder;
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
                EmailConfirmed = true
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

                return CustomResponse(await _token.TokenJwt(userRegister.Email));
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
                throw new InvalidOperationException($"Não foi possível carregar o usuário de autenticação de dois fatores.");
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

        
        
        [HttpGet("Authenticator")]
        public async Task<IActionResult> GetAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            var authentication = new TwoFactorAuthenticator
            {
                LoadSharedKeyAndQrCodeUri = await LoadSharedKeyAndQrCodeUriAsync(user),
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
                RecoveryCodes = _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).Result.ToArray()
            };           

            return CustomResponse(authentication);
        }

        [HttpPost("Authenticator")]
        public async Task<IActionResult> PostAuthenticator(TwoFactorAuthenticator twoFactor)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var verificationCode = twoFactor.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                AddErros("O código de verificação é inválido.");
                return CustomResponse();
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            twoFactor.StatusMessage = "Seu aplicativo autenticador foi verificado.";

            var authentication = new TwoFactorAuthenticator { LoadSharedKeyAndQrCodeUri = await LoadSharedKeyAndQrCodeUriAsync(user) };
            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                authentication.RecoveryCodes = recoveryCodes.ToArray();
            }
            return CustomResponse(authentication);
        }

        [HttpPost("ResetAuthenticator")]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);            
            await _signInManager.RefreshSignInAsync(user);            
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
                                                    userRegister.Cpf));
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }

        }

        private async Task<LoadSharedKeyAndQrCodeUri> LoadSharedKeyAndQrCodeUriAsync(IdentityUser user)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var email = await _userManager.GetEmailAsync(user);
            return new LoadSharedKeyAndQrCodeUri
            {
                AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey),
                SharedKey = FormatKey(unformattedKey)
            };
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("teste"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }

}
