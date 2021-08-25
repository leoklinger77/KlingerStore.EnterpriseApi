using KSE.WebAppMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace KSE.WebAppMvc.Extensions.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedViewModel modelPagination)
        {
            return View(modelPagination);
        }
    }
}
