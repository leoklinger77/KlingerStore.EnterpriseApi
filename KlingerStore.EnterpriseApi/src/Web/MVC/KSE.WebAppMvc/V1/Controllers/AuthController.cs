using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    [Authorize]
    public class AuthController : MainController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;

        }

        [AllowAnonymous]
        [HttpGet("nova-conta")]

        public async Task<IActionResult> Register(string returnUrl = null)
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost("nova-conta")]

        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);

            var rs = await _authService.Register(userRegister);

            if (HasErrorResponse(rs.ResponseResult)) return View(userRegister);

            await _authService.RealizarLogin(rs);

            return RedirectToAction("Index", "Catalog");
        }

        [AllowAnonymous]
        [HttpGet("Login")]

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(userLogin);

            var rs = await _authService.Login(userLogin);
            if (rs.ResponseResult != null && (bool)(rs.ResponseResult?.errors.Messagens.Contains("2FA Requerido.")))
            {
                return RedirectToAction("LoginWith2fa", new { Email = userLogin.Email, ReturnUrl = returnUrl });
            }
            if (HasErrorResponse(rs.ResponseResult)) return View(userLogin);

            await _authService.RealizarLogin(rs);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");

            return LocalRedirect(returnUrl);

        }        

        [AllowAnonymous]
        [HttpGet("LoginWith2fa")]
        public async Task<IActionResult> LoginWith2fa([Required] string email, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(email))
            {
                AddError("E-mail requido para 2fa.");
                return View("Login");
            }

            return View(new UserLoginWith2fa { Email = email });
        }

        [AllowAnonymous]
        [HttpPost("LoginWith2fa")]
        public async Task<IActionResult> LoginWith2fa(UserLoginWith2fa with2Fa, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View("LoginWith2fa", ModelState);

            var rs = await _authService.LoginWith2fa(with2Fa);

            if (HasErrorResponse(rs.ResponseResult)) return View("LoginWith2fa");

            await _authService.RealizarLogin(rs);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");

            return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        [HttpGet("LoginWithRecoveryCode")]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;           
            return View();
        }
        
        [HttpGet("sair")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();

            return RedirectToAction("Index", "Catalog");
        }



        [HttpGet("GenerateRecovery")]
        public async Task<IActionResult> GenerateRecovery()
        {
            return View();
        }
        [HttpPost("ShowRecoveryCodes")]
        public async Task<IActionResult> ShowRecoveryCodes()
        {
            return View(await _authService.GenerateRecovery());
        }


        [HttpGet("2fa-authenticacao")]
        public async Task<IActionResult> TwoTactorAuthentication()
        {
            var result = await _authService.GetAuthenticator();
            return View(result);
        }

        [HttpPost("2fa-AuthenticationVerified")]
        public async Task<IActionResult> TwoTactorAuthenticationVerified(TwoFactorAuthenticator twoFactor)
        {
            if (!ModelState.IsValid) return View("EnableAuthenticator", ModelState);
            var result = await _authService.AuthenticatorVerified(twoFactor);

            if(HasErrorResponse(result)) return View("EnableAuthenticator");


            return RedirectToAction("TwoTactorAuthentication");
        }

        [HttpGet("2fa-EnableAuthenticator")]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var result = await _authService.GetAuthenticator();
            return View(result);
        }


        [HttpGet("2fa-ResetAuthenticator")]
        public async Task<IActionResult> ResetAuthenticator()
        {            
            return View();
        }

        [HttpPost("2fa-ResetAuthenticator")]
        public async Task<IActionResult> PostResetAuthenticator()
        {
            var response = await _authService.ResetAuthenticator();

            if (HasErrorResponse(response)) return View("TwoTactorAuthentication");

            TempData["Erro"] = "Sua chave de aplicativo autenticador foi redefinida, você precisará configurar seu aplicativo autenticador usando a nova chave.";
            return RedirectToAction("EnableAuthenticator");
        }

        
        
        [HttpGet("seguranca")]
        public async Task<IActionResult> Security()
        {
            return View();
        }

        [HttpPost("seguranca")]
        public async Task<IActionResult> Security(UserUpdatePassword updatePassword)
        {
            if (!ModelState.IsValid) return View(ModelState);




            return View();
        }
    }
}
