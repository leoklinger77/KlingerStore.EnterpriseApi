using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace KSE.WebApi.Core.User
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string UserName => _accessor.HttpContext.User.Identity.Name;

        public Guid UserId => IsAuthentication() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

        public IEnumerable<Claim> FindClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext FindHttpContext()
        {
            return _accessor.HttpContext;
        }

        public string GetRefreshToken()
        {
            return IsAuthentication() ? _accessor.HttpContext.User.GetRefreshToken() : "";
        }

        public string GetUserEmail()
        {
            return IsAuthentication() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public string GetUserToken()
        {
            return IsAuthentication() ? _accessor.HttpContext.User.GetUserToken() : "";
        }

        public bool HasRoles(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public bool IsAuthentication()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
