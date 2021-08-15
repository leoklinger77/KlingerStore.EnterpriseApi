using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    public class AuthController : MainController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("nova-conta")]
        
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            
            return View();
        }

        [HttpPost("nova-conta")]
        
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);

            var rs = await _authService.Register(userRegister);

            if (HasErrorResponse(rs.ResponseResult)) return View(userRegister);

            await RealizarLogi(rs);

            return RedirectToAction("Index", "Catalog");
        }

        [HttpGet("Login")]
        
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]        
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(userLogin);

            var rs = await _authService.Login(userLogin);
            
            if (HasErrorResponse(rs.ResponseResult)) return View(userLogin);

            await RealizarLogi(rs);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");

            return LocalRedirect(returnUrl);
            
        }

        [HttpGet("sair")]        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);            
            
            return RedirectToAction("Index", "Catalog");
        }

        private async Task RealizarLogi(UserResponseLogin userResponseLogin)
        {
            var token = FindTokenFormat(userResponseLogin.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", userResponseLogin.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperty = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperty);
        }

        private static JwtSecurityToken FindTokenFormat(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwtToken) as JwtSecurityToken;
        }
    }
}
