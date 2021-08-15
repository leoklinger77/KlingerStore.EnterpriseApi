using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Client.Application.Events
{
    public class ClientEventHandler : INotificationHandler<RegisteredCustomerEvent>
    {
        public Task Handle(RegisteredCustomerEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
