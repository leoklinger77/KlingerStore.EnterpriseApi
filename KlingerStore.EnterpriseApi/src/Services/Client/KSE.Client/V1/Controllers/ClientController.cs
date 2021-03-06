using KSE.Client.Application.Commands;
using KSE.Client.Application.Querys;
using KSE.Client.ViewModels;
using KSE.Core.Mediatr;
using KSE.WebApi.Core.Controllers;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Client.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:ApiVersion}/Client")]
    public class ClientController : MainController
    {
        private readonly IClientQuery _clientQuery;
        private readonly IAspNetUser _aspNetUser;
        private readonly IMediatrHandler _mediatrHandler;
        public ClientController(IClientQuery clientQuery, IAspNetUser aspNetUser, IMediatrHandler mediatrHandler)
        {
            _clientQuery = clientQuery;
            _aspNetUser = aspNetUser;
            _mediatrHandler = mediatrHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient()
        {
            return CustomResponse(await _clientQuery.GetClient(_aspNetUser.UserId));
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddress()
        {
            return CustomResponse(await _clientQuery.GetAddress(_aspNetUser.UserId));
        }

        [HttpPost("address")]
        public async Task<IActionResult> PostAddress(AddressViewModel address)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var command = new AddressCommandHandler(address.Street, address.Number, address.Complement, address.District, address.ZipCode, address.City, address.State);
            command.ClientId = _aspNetUser.UserId;
            return CustomResponse(await _mediatrHandler.SendCommand(command));
        }

        [HttpPost("Profile")]
        public async Task<IActionResult> UpdateProfile(ClientViewModel client)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            List<PhoneCommandHandler> listPhones = new List<PhoneCommandHandler>();
            foreach (var item in client.Phones)
            {
                listPhones.Add(new PhoneCommandHandler(_aspNetUser.UserId, item.Ddd, item.Number, item.PhoneType));
            }

            var command = new UpdateClientProfileCommand(_aspNetUser.UserId, client.Name, client.Cpf, client.Email, listPhones);

            return CustomResponse(await _mediatrHandler.SendCommand(command));
        }
    }
}
