using KSE.Catalog.Interfaces;
using KSE.Catalog.Models;
using KSE.WebApi.Core.Controllers;
using KSE.WebApi.Core.Identity;
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
        [HttpGet("Product")]        
        public async Task<IEnumerable<Product>> Index()
        {
            return await _productRepository.FindAllProduct();
        }
                
        [HttpGet("Product/{id}")]        
        public async Task<Product> GetByProductId(Guid id)
        {            
            return await _productRepository.FindByIdProduct(id);
        }
    }
}
