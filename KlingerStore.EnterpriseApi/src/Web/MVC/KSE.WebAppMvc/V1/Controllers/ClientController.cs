using KSE.WebAppMvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    [Route("client")]
    public class ClientController : MainController
    {
        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MyOrders()
        {
            return View();
        }

        [HttpGet("meu-perfil")]
        public async Task<IActionResult> Profile()
        {
            return View();
        }

        [HttpGet("endereco")]
        public async Task<IActionResult> Address()
        {
            return View();
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
    }
}
