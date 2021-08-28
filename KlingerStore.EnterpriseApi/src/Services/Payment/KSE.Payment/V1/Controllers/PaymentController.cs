using KSE.Payment.Interfaces;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KSE.Payment.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:ApiVersion}/Payment")]
    public class PaymentController : MainController
    {
        private readonly ITaxaTransactionRepository _transactionRepository;

        public PaymentController(ITaxaTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        
        [HttpGet("taxa")]
        public async Task<IActionResult> GetTaxaTransaction()
        {
            var taxas = await _transactionRepository.GetFindAll();

            if(taxas == null)
            {
                AddErros("Taxas não localizadas.");
                return CustomResponse();
            }

            return CustomResponse(taxas);
        }
    }
}
