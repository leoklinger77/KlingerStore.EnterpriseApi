using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.Client.V1.Controllers
{
    [Route("Client")]
    public class ClientController : MainController
    {
        public ClientController()
        {            
        }

        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            return CustomResponse();
        }
    }
}
