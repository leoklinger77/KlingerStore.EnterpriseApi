using KSE.Core.Communication;
using KSE.WebAppMvc.Models;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientViewModel> GetClient();
        Task<AddressViewModel> GetAddress();
        Task<ResponseResult> CreateAddress(AddressViewModel address);
    }
}
