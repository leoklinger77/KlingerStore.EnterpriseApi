using KSE.Core.Communication;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KSE.WebAppMvc.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool HasErrorResponse(ResponseResult response)
        {
            if(response != null && response.errors.Messagens.Any())
            {
                foreach (var item in response.errors.Messagens)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
                return true;
            }
            return false;
        }

        protected void AddError(string message)
        {
            ModelState.AddModelError(string.Empty, message);
        }
        protected bool OperationValid()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
