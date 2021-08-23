using System;
using System.ComponentModel.DataAnnotations;

namespace KSE.WebAppMvc.Extensions.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ExpirationCartAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null) return false;

            var month = value.ToString().Split('/')[0];
            var yaer = $"20{value.ToString().Split('/')[1]}";
            if (int.TryParse(month, out var m) && int.TryParse(yaer, out var y))
            {
                var date = new DateTime(y, m, 1);
                return date > DateTime.Now;
            }
            return false;
        }
    }
}
