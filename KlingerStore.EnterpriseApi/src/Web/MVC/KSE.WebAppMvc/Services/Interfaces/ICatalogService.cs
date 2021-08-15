using KSE.WebAppMvc.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> FindAll();
        Task<ProductViewModel> FindById(Guid id);
    }
}
