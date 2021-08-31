using KSE.Gateway.Purchase.Models.Client;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientDTO> GetClient();
    }
}
