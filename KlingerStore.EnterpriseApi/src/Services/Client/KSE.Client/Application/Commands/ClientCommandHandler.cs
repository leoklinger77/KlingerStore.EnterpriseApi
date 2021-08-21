using FluentValidation.Results;
using KSE.Client.Application.Events;
using KSE.Client.Models;
using KSE.Client.Models.Interfaces;
using KSE.Core.Messages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Client.Application.Commands
{
    public class ClientCommandHandler : CommandHandler, 
                                        IRequestHandler<RegisterClientCommand, ValidationResult>,
                                        IRequestHandler<AddressCommandHandler, ValidationResult>
    {
        private readonly IClientRepository _clientRepository;

        public ClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ValidationResult> Handle(RegisterClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var client = new Models.Client(message.Id, message.Name, message.Email, message.Cpf);

            if (await _clientRepository.FindByCpf(client.Cpf.Numero) != null)
            {
                AddError("Esé CPF já está em uso.");
                return ValidationResult;
            }

            await _clientRepository.Insert(client);

            client.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));

            return await PersistData(_clientRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AddressCommandHandler message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;            

            var address = new Address(message.Street, message.Number, message.Complement, message.District, message.ZipCode, message.City, message.State);

            address.AddCliente(message.ClientId);

            await _clientRepository.Insert(address);

            return await PersistData(_clientRepository.UnitOfWork);
        }
    }
}
