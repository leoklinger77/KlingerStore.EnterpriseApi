using KSE.Core.Communication;
using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    [Route("client")]
    public class ClientController : MainController
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MyOrders()
        {
            return View();
        }

        [HttpGet("meu-perfil")]
        public async Task<IActionResult> Profile()
            => View(await _clientService.GetClient());


        [HttpGet("endereco")]
        public async Task<IActionResult> Address()
        {
            var address = await _clientService.GetAddress();           

            return View(address is ResponseResult ? new AddressViewModel() : address);
        }
            

        [HttpGet("seguranca")]
        public async Task<IActionResult> Security()
        {
            return View();
        }

        [HttpGet("devolucoes-reembolsos")]
        public async Task<IActionResult> ReturnsAndRefunds()
        {
            return View();
        }
        [HttpGet("produtos-favoritos")]
        public async Task<IActionResult> FavoriteProduct()
        {
            return View();
        }


    }
}
