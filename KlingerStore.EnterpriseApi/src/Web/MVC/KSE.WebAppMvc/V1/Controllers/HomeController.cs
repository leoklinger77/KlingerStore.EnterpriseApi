using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace KSE.WebAppMvc.V1.Controllers
{
    public class HomeController : MainController
    {
        
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet("sistema-indisponivel")]
        public IActionResult SystemUnavailable()
        {            
            return View("Error", new ErrorViewModel
            {
                Message = "O sistem está temporariamente indisponível, isto pode ocorrer em momento de sobrecarga de usuário.",
                Title = "Sistema indisponível.",
                ErroCode = 500
            });
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
