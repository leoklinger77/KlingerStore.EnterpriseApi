using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    [Authorize]
    public class TwoFactorController : MainController
    {
        private readonly IAuthService _authService;

        public TwoFactorController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("GenerateRecovery")]
        public async Task<IActionResult> GenerateRecovery()
        {
            return View();
        }

        [HttpPost("ShowRecoveryCodes")]
        public async Task<IActionResult> ShowRecoveryCodes()
        {
            var result = await _authService.GenerateRecovery();

            return View(result);
        }

        [HttpGet("authenticacao")]
        public async Task<IActionResult> GetTwoFactorAuthentication()
        {
            var result = await _authService.GetAuthenticator();
            return View(result);
        }

        [HttpPost("AuthenticationVerified")]
        public async Task<IActionResult> PostTwoFactorAuthentication(TwoFactorAuthenticator twoFactor)
        {
            if (!ModelState.IsValid) return View("EnableAuthenticator", ModelState);
            var result = await _authService.AuthenticatorVerified(twoFactor);

            if (HasErrorResponse(result))
            {                
                return View("EnableAuthenticator", await _authService.GetAuthenticator());
            }

            return RedirectToAction("GetTwoFactorAuthentication");
        }

        [HttpGet("EnableAuthenticator")]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var result = await _authService.GetAuthenticator();
            return View(result);
        }

        [HttpGet("ResetAuthenticator")]
        public async Task<IActionResult> ResetAuthenticator()
        {
            return View();
        }

        [HttpPost("ResetAuthenticator")]
        public async Task<IActionResult> PostResetAuthenticator()
        {
            var response = await _authService.ResetAuthenticator();

            if (HasErrorResponse(response)) return View("GetTwoFactorAuthentication");

            TempData["Erro"] = "Sua chave de aplicativo autenticador foi redefinida, você precisará configurar seu aplicativo autenticador usando a nova chave.";
            return RedirectToAction("EnableAuthenticator");
        }

        [HttpGet("Disable2fa")]        
        public async Task<IActionResult> Disable2fa()
        {
            var response = await _authService.Disable2fa();

            if (HasErrorResponse(response)) return View("GetTwoFactorAuthentication");

            TempData["Erro"] = "Sua chave de aplicativo autenticador foi redefinida, você precisará configurar seu aplicativo autenticador usando a nova chave.";
            return RedirectToAction("EnableAuthenticator");
            
        }
    }
}
