using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace KSE.WebAppMvc.Extensions.Polly
{
    public static class PollyExtension
    {
        public static AsyncRetryPolicy<HttpResponseMessage> WaitAndTry()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            });
        }
    }
}
