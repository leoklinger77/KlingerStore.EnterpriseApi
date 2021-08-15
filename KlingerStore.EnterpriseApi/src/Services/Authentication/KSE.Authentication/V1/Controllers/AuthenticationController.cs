using EasyNetQ;
using KSE.Authentication.Extensions;
using KSE.Authentication.Models;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Core.Messages.IntegrationEvents.Client;
using KSE.MessageBus;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KSE.Authentication.V1.Controllers
{
    [Route("V1/Authentication")]
    public class AuthenticationController : MainController
    {
        private readonly IMessageBus _bus;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GeneretorToken _token;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, GeneretorToken token, IMessageBus bus)
        {
            _bus = bus;
            _signInManager = signInManager;
            _userManager = userManager;
            _token = token;            
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
    }
}
