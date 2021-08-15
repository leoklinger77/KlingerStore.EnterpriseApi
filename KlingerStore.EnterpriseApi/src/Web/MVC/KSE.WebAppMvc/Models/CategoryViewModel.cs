using System;
using System.Collections.Generic;

namespace KSE.WebAppMvc.Models
{
    public class CategoryViewModel 
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public int Code { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
        protected CategoryViewModel() { }
        public CategoryViewModel(string name, int code)
        {
            Name = name;
            Code = code;            
        }   
    }
}
