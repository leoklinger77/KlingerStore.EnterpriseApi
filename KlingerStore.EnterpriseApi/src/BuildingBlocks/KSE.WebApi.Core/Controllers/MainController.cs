using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace KSE.WebApi.Core.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();
        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationVation())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messagens" , Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AddErros(erros.ToString());
            }
            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult modelState)
        {
            foreach (var item in modelState.Errors)
            {
                AddErros(item.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool OperationVation()
        {
            return !Erros.Any();
        }
        protected void AddErros(string erro)
        {
            Erros.Add(erro);
        }
        protected void ClearErros()
        {
            Erros.Clear();
        }
    }
}
