using KSE.WebAppMvc.Controllers;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.V1.Controllers
{
    public class CatalogController : MainController
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("")]        
        [Route("Vitrine")]
        public async Task<IActionResult> Index([FromQuery] int pageSize = 8, [FromQuery] int pageIndex = 1, [FromQuery] string query = null)
        {
            var products = await _catalogService.FindAll(pageSize, pageIndex, query);
            ViewBag.Search = query;
            products.ReferenceAction = "Index";
            return View(products);
        }

        [HttpGet("produto-detalhe/{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            return View(await _catalogService.FindById(id));
        }
    }
}
