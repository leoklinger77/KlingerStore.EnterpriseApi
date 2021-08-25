using KSE.Client.Models;
using KSE.Client.Models.Interfaces;
using KSE.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Client.Data.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientContext _clientContext;

        public ClientRepository(ClientContext clientContext)
        {
            _clientContext = clientContext;
        }
        public IUnitOfWork UnitOfWork => _clientContext;

        public async Task<IEnumerable<Models.Client>> FindAll()
        {
            return await _clientContext.Client.AsNoTracking().ToListAsync();
        }

        public async Task<Models.Client> FindByCpf(string cpf)
        {
            return await _clientContext.Client.AsNoTracking().FirstOrDefaultAsync(x => x.Cpf.Numero == cpf);
        }

        public async Task Insert(Models.Client client)
        {
            await _clientContext.AddAsync(client);
        }
        public void Dispose()
        {
            _clientContext?.DisposeAsync();
        }

        public async Task<Models.Client> GetClient(Guid id)
        {
            return await _clientContext.Client
                .Include(x => x.Address)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Address> GetAddress(Guid id)
        {
            return await _clientContext.Address.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == id);
        }

        public async Task Insert(Address address)
        {
            await _clientContext.Address.AddAsync(address);
        }
    }
}
