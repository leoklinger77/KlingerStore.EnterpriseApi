namespace KSE.WebAppMvc.Models
{
    public class ClientViewModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }        
        public AddressViewModel Address { get; set; }
    }    
}
