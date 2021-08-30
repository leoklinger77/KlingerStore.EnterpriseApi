using KSE.Client.Models.Enum;
using System.Collections.Generic;

namespace KSE.Client.ViewModels
{
    public class ClientViewModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public ClientStatus Status { get; set; }
        public AddressViewModel Address { get; set; }
        public List<PhoneViewModel> Phones { get; set; } = new List<PhoneViewModel>();

    }

    public class UpdateProfile
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }

        public string Celular { get; set; }
        public string Telefone { get; set; }
    }
}
