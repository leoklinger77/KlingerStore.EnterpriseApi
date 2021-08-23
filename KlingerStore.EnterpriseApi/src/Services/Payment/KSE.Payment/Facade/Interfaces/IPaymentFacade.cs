using KSE.Payment.Models;
using System.Threading.Tasks;

namespace KSE.Payment.Facade.Interfaces
{
    public interface IPaymentFacade
    {
        Task<Transaction> AuthorizePayment(Models.Payment payment);
        Task<Transaction> CapturePayment(Transaction transaction);
        Task<Transaction> CanceledPayment(Transaction transaction);
    }
}
