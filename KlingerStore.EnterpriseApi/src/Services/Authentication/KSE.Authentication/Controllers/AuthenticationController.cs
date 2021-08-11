using KSE.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.Authentication.Controllers
{
    [ApiController]
    [Route("Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("FirstAccess")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);

            if (result.Succeeded)
            {
                return Ok();
            }
            else if (result.IsLockedOut)
            {
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
