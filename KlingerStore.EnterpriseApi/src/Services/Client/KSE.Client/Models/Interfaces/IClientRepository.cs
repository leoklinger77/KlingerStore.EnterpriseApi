using KSE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Client.Models.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task Insert(Client client);
        Task<IEnumerable<Client>> FindAll();
        Task<Client> FindByCpf(string cpf);
        
        Task<Client> GetClient(Guid id);
        Task<Address> GetAddress(Guid id);
    }
}
