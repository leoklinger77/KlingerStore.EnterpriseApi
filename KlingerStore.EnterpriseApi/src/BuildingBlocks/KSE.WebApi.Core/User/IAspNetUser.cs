using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace KSE.WebApi.Core.User
{
    public interface IAspNetUser
    {
        string UserName { get; }
        Guid UserId { get; }
        string GetUserEmail();
        string GetUserToken();
        string GetRefreshToken();
        bool IsAuthentication();
        bool HasRoles(string role);
        IEnumerable<Claim> FindClaims();
        HttpContext FindHttpContext();
    }
}
