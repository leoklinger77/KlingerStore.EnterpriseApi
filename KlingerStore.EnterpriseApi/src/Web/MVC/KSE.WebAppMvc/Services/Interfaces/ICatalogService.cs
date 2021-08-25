using KSE.WebAppMvc.Models;
using System;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<PagedViewModel<ProductViewModel>> FindAll(int pageSize, int pageIndex, string query = null);
        Task<ProductViewModel> FindById(Guid id);
    }
}
