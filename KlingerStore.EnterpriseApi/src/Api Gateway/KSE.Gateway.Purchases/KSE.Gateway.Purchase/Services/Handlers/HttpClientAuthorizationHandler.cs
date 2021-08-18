using KSE.WebApi.Core.User;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Gateway.Purchase.Services.Handlers
{
    public class HttpClientAuthorizationHandler : DelegatingHandler
    {
        private readonly IAspNetUser _user;
        private const string Authorization = "Authorization";               
        public HttpClientAuthorizationHandler(IAspNetUser user)
        {
            _user = user;
        }

        private string FindToken => _user.FindUserToken();

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
