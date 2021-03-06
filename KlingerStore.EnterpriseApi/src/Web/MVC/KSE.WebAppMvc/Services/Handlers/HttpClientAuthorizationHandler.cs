using KSE.WebAppMvc.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Services.Handlers
{
    public class HttpClientAuthorizationHandler : DelegatingHandler
    {
        private readonly IUser _user;
        private const string Authorization = "Authorization";               
        public HttpClientAuthorizationHandler(IUser user)
        {
            _user = user;
        }

        private string FindToken => _user.GetUserToken();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _user.FindHttpContext().Request.Headers[Authorization];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add(Authorization, new List<string>() { authorizationHeader });
            }           

            if (FindToken != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", FindToken);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
