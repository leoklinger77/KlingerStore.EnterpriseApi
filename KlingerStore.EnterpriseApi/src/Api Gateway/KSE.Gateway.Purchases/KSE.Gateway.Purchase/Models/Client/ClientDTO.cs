namespace KSE.Gateway.Purchase.Models.Client
{
    public class ClientDTO
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public AddressDTO Address { get; set; }
    }
}
