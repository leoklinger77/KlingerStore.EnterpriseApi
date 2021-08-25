using KSE.Catalog.Interfaces;
using KSE.Catalog.Models;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Catalog.V1.Controllers
{
    [Authorize]    
    [Route("V1/Catalog")]
    public class CatalogController : MainController
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet("Product/")]        
        public async Task<PagedResult<Product>> Index([FromQuery] int pageSize = 8, [FromQuery]int pageIndex = 1, [FromQuery] string query = null)
        {
            return await _productRepository.FindAllProduct(pageSize, pageIndex, query);
        }
                
        [HttpGet("Product/{id}")]        
        public async Task<Product> GetByProductId(Guid id)
        {            
            return await _productRepository.FindByIdProduct(id);
        }
        [HttpGet("Product/list/{productIds}")]
        public async Task<IEnumerable<Product>> ObterProdutosPorId(string productIds)
        {
            return await _productRepository.FindAllProductPerIds(productIds);
        }
    }
}
