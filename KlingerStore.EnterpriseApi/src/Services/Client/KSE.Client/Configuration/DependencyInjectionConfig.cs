using FluentValidation.Results;
using KSE.Client.Application.Commands;
using KSE.Client.Application.Events;
using KSE.Client.Data;
using KSE.Client.Data.Repository;
using KSE.Client.Models.Interfaces;
using KSE.Client.Services;
using KSE.Core.Mediatr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Client.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            //Command
            services.AddScoped<IMediatrHandler, MediatrHandler>();
            services.AddScoped<IRequestHandler<RegisterClientCommand, ValidationResult>, ClientCommandHandler>();

            //Event
            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, ClientEventHandler>();
                         
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ClientContext>();            
        }
    }
}
