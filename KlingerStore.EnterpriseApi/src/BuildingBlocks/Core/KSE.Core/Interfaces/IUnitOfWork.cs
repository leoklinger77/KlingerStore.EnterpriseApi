using System.Threading.Tasks;

namespace KSE.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
