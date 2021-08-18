using KSE.Client.Models.Enum;
using KSE.Core.DomainObjets;

namespace KSE.Client.ViewModels
{
    public class ClientViewModel
    {
        public string Name { get; set; }
        public Cpf Cpf { get; set; }
        public Email Email { get; set; }
        public ClientStatus Status { get; set; }
        public AddressViewModel Address { get; set; }

    }
}
