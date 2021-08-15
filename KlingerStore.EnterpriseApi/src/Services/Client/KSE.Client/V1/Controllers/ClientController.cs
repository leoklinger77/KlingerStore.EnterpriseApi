using KSE.Client.Application.Commands;
using KSE.Core.Mediatr;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KSE.Client.V1.Controllers
{
    [Route("Client")]
    public class ClientController : MainController
    {
        private readonly IMediatrHandler _mediatrHandler;

        public ClientController(IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
        }

        [HttpGet("Clients")]
        public async Task<IActionResult> Insert()
        {
            return CustomResponse(await _mediatrHandler.SendCommand(new RegisterClientCommand(Guid.NewGuid(), "Leandro", "36018556820", "leandro@gmail.com")));
        }
    }
}
