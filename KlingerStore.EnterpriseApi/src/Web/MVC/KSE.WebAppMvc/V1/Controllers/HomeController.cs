using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSE.WebAppMvc.V1.Controllers
{
    public class HomeController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
                
        [HttpGet("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Title = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Message = "A página que está procurando não existe! <br/>Em caso de dúvida entre em contato com nosso suporte.";
                modelErro.Title = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;

            }
            else if (id == 403)
            {
                modelErro.Message = "Você não tem permissão para fazer isto.";
                modelErro.Title = "Acesso negado!";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}
