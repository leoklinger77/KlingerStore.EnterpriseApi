using FluentValidation.Results;
using KSE.Client.Application.Events;
using KSE.Client.Models;
using KSE.Client.Models.Enum;
using KSE.Client.Models.Interfaces;
using KSE.Core.DomainObjets;
using KSE.Core.Messages;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Client.Application.Commands
{
    public class ClientCommandHandler : CommandHandler,
                                        IRequestHandler<RegisterClientCommand, ValidationResult>,
                                        IRequestHandler<AddressCommandHandler, ValidationResult>,
                                        IRequestHandler<UpdateClientProfileCommand, ValidationResult>
    {
        private readonly IClientRepository _clientRepository;

        public ClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ValidationResult> Handle(RegisterClientCommand message, CancellationToken cancellationToken)
        {
            try
            {


                if (!message.IsValid()) return message.ValidationResult;

                var client = new Models.Client(message.Id, message.Name, message.Email, message.Cpf);
                var phone = new Phone(message.Ddd, message.PhoneNumber, (PhoneType)1);

                client.AddPhone(phone);

                if (await _clientRepository.FindByCpf(client.Cpf.Numero) != null)
                {
                    AddError("Esé CPF já está em uso.");
                    return ValidationResult;
                }

                await _clientRepository.Insert(client);

                client.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));

                return await PersistData(_clientRepository.UnitOfWork);

            }
            catch (System.Exception e)
            {

                throw;
            }
        }

        public async Task<ValidationResult> Handle(AddressCommandHandler message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var address = new Address(message.Street, message.Number, message.Complement, message.District, message.ZipCode, message.City, message.State);

            address.AddCliente(message.ClientId);

            await _clientRepository.Insert(address);

            return await PersistData(_clientRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateClientProfileCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var clientDb = await _clientRepository.GetClient(message.AggregateId);

            if (clientDb is null)
            {
                throw new DomainException("Client não localizado.");
            }

            clientDb.ChangeName(message.Name);

            if (message.Phones.Count >= 3)
            {
                AddError("Permitido no máximo 2 telefones");
                return ValidationResult;
            }

            var cel = message.Phones.Where(x => x.PhoneType == 1).FirstOrDefault();
            var tel = message.Phones.Where(x => x.PhoneType == 2).FirstOrDefault();

            if (cel is null)
            {
                AddError("É obrigatorio 1 Celular");
                return ValidationResult;
            }

            if (tel is null)
            {
                var telDb =  clientDb.Phones.Where(x => x.PhoneType == PhoneType.Home).FirstOrDefault();
                if(telDb != null)
                {
                    clientDb.Phones.Remove(telDb);
                    await _clientRepository.DeletePhone(telDb);
                }
            }

            foreach (var item in clientDb.Phones)
            {
                if (item.PhoneType == PhoneType.Cell)
                {
                    if (item.Number != cel.Number)
                    {
                        item.ChangeDdd(cel.Ddd);
                        item.ChangeNumber(cel.Number);
                    }
                }

                if (item.PhoneType == PhoneType.Home)
                {
                    if (item.Number != cel.Number)
                    {
                        item.ChangeDdd(cel.Ddd);
                        item.ChangeNumber(cel.Number);
                    }
                }
            }

            if (clientDb.Phones.Count == 1 && tel != null)
            {
                var phone = new Phone(tel.Ddd, tel.Number, PhoneType.Home);
                phone.AddCliente(clientDb.Id);
                clientDb.Phones.Add(phone);
                await _clientRepository.Insert(phone);
            }

            await _clientRepository.Update(clientDb);

            return await PersistData(_clientRepository.UnitOfWork);
        }
    }
}
