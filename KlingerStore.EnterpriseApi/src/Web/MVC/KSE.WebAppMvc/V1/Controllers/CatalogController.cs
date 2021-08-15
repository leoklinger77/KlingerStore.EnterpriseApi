using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    public class CatalogController : MainController
    {
        private readonly ICatalogServiceRefit _catalogService;

        public CatalogController(ICatalogServiceRefit catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("")]        
        [Route("Vitrine")]
        public async Task<IActionResult> Index()
        {            
            return View(await _catalogService.FindAll());
        }

        [HttpGet("produto-detalhe/{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            return View(await _catalogService.FindById(id));
        }
    }
}
