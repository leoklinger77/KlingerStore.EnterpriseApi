using KSE.Order.Application.DTO;
using KSE.Order.Application.Querys;
using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace KSE.Order.V1.Controllers
{
    [Authorize]
    [Route("v1/voucher")]
    public class VoucherController : MainController
    {
        private readonly IVoucherQuery _voucherQuery;

        public VoucherController(IVoucherQuery voucherQuery)
        {            
            _voucherQuery = voucherQuery;
        }
        
        [HttpGet("{code}")]
        [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPerCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return NotFound();

            var voucher = await _voucherQuery.GetVoucherPerCode(code);

            return voucher == null ? NotFound() : CustomResponse(voucher);
        }
    }
}
