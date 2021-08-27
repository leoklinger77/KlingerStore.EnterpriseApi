using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace KSE.WebAppMvc.Extensions
{
    public interface IUser
    {
        string UserName { get; }
        Guid UserId { get; }
        string GetUserEmail();
        string GetUserToken();       
        bool IsAuthentication();
        bool HasRoles(string role);
        IEnumerable<Claim> FindClaims();
        HttpContext FindHttpContext();
    }
}
