using System;

namespace KSE.Client.ViewModels
{
    public class PhoneViewModel
    {
        public Guid Id { get; set; }
        public string Ddd { get; set; }
        public string Number { get; set; }
        public int PhoneType { get; set; }
    }
}
