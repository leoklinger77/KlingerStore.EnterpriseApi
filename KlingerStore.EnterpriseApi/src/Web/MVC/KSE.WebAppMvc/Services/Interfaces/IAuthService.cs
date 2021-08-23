using KSE.Core.Communication;
using KSE.WebAppMvc.Models;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);
        Task<UserResponseLogin> Register(UserRegister userRegister);
        Task<TwoFactorAuthenticator> GetAuthenticator();
        Task<ResponseResult> AuthenticatorVerified(TwoFactorAuthenticator twoFactor);
        Task<UserResponseLogin> LoginWith2fa(UserLoginWith2fa with2Fa);
        Task<ResponseResult> ResetAuthenticator();
        Task<string[]> GenerateRecovery();
    }
}
