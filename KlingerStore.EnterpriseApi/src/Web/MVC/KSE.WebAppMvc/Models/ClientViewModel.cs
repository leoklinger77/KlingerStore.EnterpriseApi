using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KSE.WebAppMvc.Models
{
    public class ClientViewModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }        
        public AddressViewModel Address { get; set; }
        public List<PhoneViewModel> Phones { get; set; } = new List<PhoneViewModel>();
    }   

    public class ClientProfileViewModel
    {
        [Required]
        public string PrimaryName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Guid CelId { get; set; }

        [Required]
        public string Celular { get; set; }

        public Guid TelId { get; set; }
        public string Telefone { get; set; }
    }
}
