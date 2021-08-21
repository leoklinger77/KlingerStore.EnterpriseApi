using FluentValidation.Results;
using KSE.Client.Application.Commands;
using KSE.Client.Application.Events;
using KSE.Client.Application.Querys;
using KSE.Client.Data;
using KSE.Client.Data.Repository;
using KSE.Client.Models.Interfaces;
using KSE.Core.Mediatr;
using KSE.WebApi.Core.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Client.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //Command
            services.AddScoped<IMediatrHandler, MediatrHandler>();
            services.AddScoped<IRequestHandler<RegisterClientCommand, ValidationResult>, ClientCommandHandler>();
            services.AddScoped<IRequestHandler<AddressCommandHandler, ValidationResult>, ClientCommandHandler>();            

            //Event
            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, ClientEventHandler>();
                         
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientQuery, ClientQuery>();
            services.AddScoped<ClientContext>();            
        }
    }
}
