using KSE.Authentication.Models;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KSE.Authentication.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:ApiVersion}/TwoFactor")]
    public class TwoFactorController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public TwoFactorController(SignInManager<IdentityUser> signInManager,
                                    UserManager<IdentityUser> userManager,
                                    UrlEncoder urlEncoder)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _urlEncoder = urlEncoder;
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

        [HttpGet("GenerateRecovery")]
        public async Task<IActionResult> GenerateRecovery()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            return CustomResponse(_userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).Result.ToArray());
        }

        [HttpPost("Disable2fa")]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                AddErros($"Ocorreu um erro inesperado ao desativar 2FA para o usuário com ID '{_userManager.GetUserId(User)}'.");
                return CustomResponse();
            }

            return CustomResponse();
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
