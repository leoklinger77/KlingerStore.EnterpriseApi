﻿namespace KSE.WebApi.Core.Identity
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationHours { get; set; }
        public string Issuer { get; set; }
        public string ValidIn { get; set; }
    }
}
