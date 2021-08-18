using KSE.Client.Models.Enum;
using KSE.Core.DomainObjets;

namespace KSE.Client.ViewModels
{
    public class ClientViewModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public ClientStatus Status { get; set; }
        public AddressViewModel Address { get; set; }

    }
}
