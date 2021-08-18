using System;
using System.Threading.Tasks;

namespace KSE.Client.Interfaces
{
    public interface IClientQuerys
    {
        Task<ViewModels.ClientViewModel> GetClient(Guid id);
        Task<ViewModels.AddressViewModel> GetAddress(Guid id);
    }
}
