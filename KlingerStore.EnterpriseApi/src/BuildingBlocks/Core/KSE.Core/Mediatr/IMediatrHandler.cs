using FluentValidation.Results;
using KSE.Core.Messages;
using System.Threading.Tasks;

namespace KSE.Core.Mediatr
{
    public interface IMediatrHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}
