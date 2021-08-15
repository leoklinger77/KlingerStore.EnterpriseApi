using FluentValidation.Results;
using KSE.Core.Interfaces;
using System.Threading.Tasks;

namespace KSE.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;
        public CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }
        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError("Houve um erro ao persistir os dados.");

            return ValidationResult;
        }
    }
}
