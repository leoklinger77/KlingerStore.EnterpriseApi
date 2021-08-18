using KSE.Client.ViewModels;
using System;
using System.Threading.Tasks;

namespace KSE.Client.Application.Querys
{
    public interface IClientQuery
    {
        Task<ClientViewModel> GetClient(Guid id);
        Task<AddressViewModel> GetAddress(Guid id);
    }
}
