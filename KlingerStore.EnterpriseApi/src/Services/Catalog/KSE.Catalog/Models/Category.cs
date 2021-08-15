using KSE.Core.DomainObjets;
using System.Collections.Generic;

namespace KSE.Catalog.Models
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public int Code { get; set; }
        public ICollection<Product> Products { get; set; }
        protected Category() { }
        public Category(string name, int code)
        {
            Name = name;
            Code = code;

            Validite();
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }
        public void Validite()
        {
            /*
            Validation.ValidateIsNullOrEmpty(Name, "O Campo Nome do produto não pode estar vazio");
            Validation.ValidateIfEqual(Code, 0, "O Campo Nome do produto não pode estar vazio");
            */
        }
    }
}
