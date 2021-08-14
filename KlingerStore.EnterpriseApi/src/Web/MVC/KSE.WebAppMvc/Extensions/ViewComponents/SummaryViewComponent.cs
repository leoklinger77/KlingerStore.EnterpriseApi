using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Extensions.ViewComponents
{
    public class SummaryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
    
}
