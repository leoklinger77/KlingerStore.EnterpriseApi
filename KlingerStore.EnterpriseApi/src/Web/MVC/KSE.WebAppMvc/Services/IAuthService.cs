using KSE.WebAppMvc.Models;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);
        Task<UserResponseLogin> Registro(UserRegister userRegister);
    }
}
