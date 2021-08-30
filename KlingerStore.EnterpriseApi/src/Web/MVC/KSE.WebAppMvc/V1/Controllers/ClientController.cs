using KSE.Core.Communication;
using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        [HttpGet("meu-perfil")]
        public async Task<IActionResult> Profile()
        {
            var client = await _clientService.GetClient();

            string[] name = client.Name.Split(' ');
            string lastName = null;
            for (int i = 1; i < name.Length; i++)
            {
                lastName += name[i] + " ";
            }

            var clientProfile = new ClientProfileViewModel { PrimaryName = name[0] ,LastName = lastName.Trim(), Cpf = client.Cpf, Email = client.Email };
            foreach (var item in client.Phones)
            {
                if (item.PhoneType == 1)
                {
                    clientProfile.Celular = item.Ddd + item.Number;
                    clientProfile.CelId = item.Id;
                }
                if (item.PhoneType == 2)
                {
                    clientProfile.Telefone = item.Ddd + item.Number;
                    clientProfile.TelId = item.Id;
                }
            }

            return View(clientProfile);
        }

        [HttpPost("meu-perfil")]
        public async Task<IActionResult> Profile(ClientProfileViewModel clientViewModel)
        {
            if (!ModelState.IsValid) return View("Profile", clientViewModel);

            var client = new ClientViewModel { Name = clientViewModel.PrimaryName + " " + clientViewModel.LastName, Cpf = clientViewModel.Cpf, Email = clientViewModel.Email };

            client.Phones.Add(new PhoneViewModel { Id = clientViewModel.CelId, Ddd = clientViewModel.Celular.Substring(0, 2), Number = clientViewModel.Celular.Substring(2), PhoneType = 1 });

            if (!string.IsNullOrEmpty(clientViewModel.Telefone))
            {
                client.Phones.Add(new PhoneViewModel { Id = clientViewModel.TelId, Ddd = clientViewModel.Telefone.Substring(0, 2), Number = clientViewModel.Telefone.Substring(2), PhoneType = 2 });
            }

            var response = await _clientService.UpdateClient(client);

            if (HasErrorResponse(response))
            {
                TempData["Erro"] = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage).ToArray()).FirstOrDefault();
                return RedirectToAction("Profile");
            }

            return RedirectToAction("Profile");
        }

        [HttpGet("endereco")]
        public async Task<IActionResult> Address()
        {
            var address = await _clientService.GetAddress();

            return View(address is ResponseResult ? new AddressViewModel() : address);
        }

        [HttpPost("endereco")]
        public async Task<IActionResult> PostAddress(AddressViewModel address)
        {
            if (!ModelState.IsValid) return View("Address", ModelState);

            if (HasErrorResponse(await _clientService.CreateAddress(address))) TempData["Erros"] =
                    ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();

            return RedirectToAction("ShippingAddress", "Order");
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
