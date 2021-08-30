using KSE.Core.Communication;
using KSE.WebAppMvc.Models;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);
        Task<ResponseResult> Register(UserRegister userRegister);        
        Task<UserResponseLogin> EmailConfirmation(string userId, string code);


        Task<TwoFactorAuthenticator> GetAuthenticator();
        Task<ResponseResult> AuthenticatorVerified(TwoFactorAuthenticator twoFactor);
        Task<UserResponseLogin> LoginWith2fa(UserLoginWith2fa with2Fa);
        Task<ResponseResult> ResetAuthenticator();
        Task<string[]> GenerateRecovery();





        Task Logout();
        Task RealizarLogin(UserResponseLogin user);
        bool TokenExpiration();
        Task<UserResponseLogin> RefreshToken(string refreshRoken);
        Task<bool> RefreshTokenValid();
        Task<UserResponseLogin> LoginWithRecovery(LoginWithRecovery loginWithRecovery);
        Task<ResponseResult> Disable2fa();
        
    }
}
