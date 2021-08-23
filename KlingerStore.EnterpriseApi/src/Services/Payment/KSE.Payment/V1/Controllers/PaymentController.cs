using KSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace KSE.Payment.V1.Controllers
{
    [Authorize]
    public class PaymentController : MainController
    {
    }
}
