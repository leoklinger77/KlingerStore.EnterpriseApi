using KSE.Client.Application.Querys;
using KSE.WebApi.Core.Controllers;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.Client.V1.Controllers
{
    [Authorize]
    [Route("V1/Client")]
    public class ClientController : MainController
    {
        private readonly IClientQuery _clientQuery;
        private readonly IAspNetUser _aspNetUser;
        public ClientController(IClientQuery clientQuery, IAspNetUser aspNetUser)
        {
            _clientQuery = clientQuery;
            _aspNetUser = aspNetUser;
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
    }
}
