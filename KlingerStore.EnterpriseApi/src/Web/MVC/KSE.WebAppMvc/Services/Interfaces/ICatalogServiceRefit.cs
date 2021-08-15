using KSE.WebAppMvc.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface ICatalogServiceRefit
    {
        [Get("/V1/Catalog/Product")]
        Task<IEnumerable<ProductViewModel>> FindAll();
        [Get("/V1/Catalog/Product/{id}")]
        Task<ProductViewModel> FindById(Guid id);
    }
}
