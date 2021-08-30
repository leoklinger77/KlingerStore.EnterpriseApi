using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("nova-conta")]

        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);
            userRegister.PhoneType = 1;
            var rs = await _authService.Register(userRegister);

            if (HasErrorResponse(rs)) return View(userRegister);            

            return View("ConfirmeEmail");
        }        

        [AllowAnonymous]
        [HttpGet("email-verification")]
        public async Task<IActionResult> EmailVerification([FromQuery] string userId, [FromQuery] string code)
        {

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                AddError("Usuario não localizado.");
                return View("ConfirmeEmail");
            }

            var response = await _authService.EmailConfirmation(userId, code);
            if (HasErrorResponse(response.ResponseResult))
            {                
                return View("ConfirmeEmail");
            }

            await _authService.RealizarLogin(response);

            return RedirectToAction("Index","Catalog");
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
        public async Task<IActionResult> LoginWithRecoveryCode([Required] string email, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;           
            return View(new LoginWithRecovery { Email = email });
        }

        [AllowAnonymous]
        [HttpPost("LoginWithRecoveryCode")]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecovery loginWithRecovery, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View("LoginWithRecoveryCode", ModelState);

            var result = await _authService.LoginWithRecovery(loginWithRecovery);

            if (HasErrorResponse(result.ResponseResult)) return View("LoginWithRecoveryCode");

            await _authService.RealizarLogin(result);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");

            return LocalRedirect(returnUrl);
        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();

            return RedirectToAction("Index", "Catalog");
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
