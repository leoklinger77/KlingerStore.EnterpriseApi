using System;

namespace KSE.WebAppMvc.Models
{
    public class PhoneViewModel
    {        
        public Guid Id { get; set; }
        public string Ddd { get; set; }
        public string Number { get; set; }
        public int PhoneType { get; set; }

        public override string ToString()
        {
            return $"{Ddd} {Number}";
        }
    }
}
