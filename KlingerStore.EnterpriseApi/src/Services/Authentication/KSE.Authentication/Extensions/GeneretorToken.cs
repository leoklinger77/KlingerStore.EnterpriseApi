using KSE.Authentication.Data;
using KSE.Authentication.Models;
using KSE.WebApi.Core.Identity;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KSE.Authentication.Extensions
{
    public class GeneretorToken
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly AppTokenSettings _appTokenSettings;
        private readonly ApplicationDbContext _context;

        private readonly IAspNetUser _aspNetUser;
        private readonly IJsonWebKeySetService _jsonWebKeySet;

        public GeneretorToken(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                IOptions<AppSettings> appSettings,
                                IOptions<AppTokenSettings> appTokenSettings,
                                IAspNetUser aspNetUser,
                                IJsonWebKeySetService jsonWebKeySet,
                                ApplicationDbContext context)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _aspNetUser = aspNetUser;
            _jsonWebKeySet = jsonWebKeySet;
            _appTokenSettings = appTokenSettings.Value;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<UserResponseLogin> TokenJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await GetClaimUser(claims, user);
            var encodedToken = CodificarToken(identityClaims);

            var refreshToken = await GeneretorRefreshToken(email);

            return new UserResponseLogin
            {
                AccessToken = encodedToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    Claims = claims.Select(x => new UserClaim { Type = x.Type, Value = x.Value })
                }
            };
        }

        private async Task<ClaimsIdentity> GetClaimUser(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var item in userRoles)
            {
                claims.Add(new Claim("role", item));
            }
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodificarToken(ClaimsIdentity claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jsonWebKeySet.GetCurrentSigningCredentials();

            var currentIssuer = $"{_aspNetUser.FindHttpContext().Request.Scheme}://{_aspNetUser.FindHttpContext().Request.Host}";


            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = currentIssuer,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });

            return tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<RefreshToken> GeneretorRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                UserName = email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettings.RefreshTokenExpiration)
            };

            _context.RefreshToken.RemoveRange(_context.RefreshToken.Where(x => x.UserName == email));
            await _context.RefreshToken.AddAsync(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken> GetRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshToken.AsNoTracking().FirstOrDefaultAsync(x => x.Token == refreshToken);
            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now ? token : null;
        }
    }
}
