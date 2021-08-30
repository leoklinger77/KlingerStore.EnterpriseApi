using FluentValidation.Results;
using KSE.Client.Application.Commands;
using KSE.Core.Mediatr;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Core.Messages.IntegrationEvents.Client;
using KSE.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Client.Services
{
    public class RegisterClientIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;
        
        public RegisterClientIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => await RegisteredCLient(request));

            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> RegisteredCLient(RegisteredUserIntegrationEvent message)
        {
            var clientCommand = new RegisterClientCommand(message.AggregateId, message.Name, message.Cpf, message.Email, message.Ddd, message.PhoneNumber);

            ValidationResult success;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediatrHandler>();
                success = await mediatr.SendCommand(clientCommand);
            }

            return new ResponseMessage(success);
        }
    }
}
