using KSE.Authentication.Extensions;
using KSE.Authentication.Models;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.Authentication.V1.Controllers
{
    [Route("V1/Authentication")]
    public class AuthenticationController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GeneretorToken _token;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, GeneretorToken token)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _token = token;
        }

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
                return CustomResponse(await _token.TokenJwt(userRegister.Email));
            }
            foreach (var item in result.Errors)
            {
                AddErros(item.Description);
            }
            return CustomResponse();
        }

        [HttpPost("FirstAccess")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);

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
    }
}
