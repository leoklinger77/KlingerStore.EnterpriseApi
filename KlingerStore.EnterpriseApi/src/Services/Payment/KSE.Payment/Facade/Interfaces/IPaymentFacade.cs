using KSE.Payment.Models;
using System.Threading.Tasks;

namespace KSE.Payment.Facade.Interfaces
{
    public interface IPaymentFacade
    {
        Task<ReturnPayment> AuthorizePayment(Models.Payment payment);
        Task<ReturnPayment> CanceledPayment(string tid);
        Task<ReturnPayment> CapturingPayment(string tid);
    }
}
