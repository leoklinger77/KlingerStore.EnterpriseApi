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
        string FindUserEmail();
        string FindUserToken();
        bool IsAuthentication();
        bool HasRoles(string role);
        IEnumerable<Claim> FindClaims();
        HttpContext FindHttoContext();
    }
}
