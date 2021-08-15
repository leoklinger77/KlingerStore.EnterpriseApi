using System;

namespace KSE.WebAppMvc.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get;  set; }
        public CategoryViewModel Category { get;  set; }
        public Dimension Dimension { get; set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public bool Active { get;  set; }
        public decimal Value { get;  set; }
        public DateTime InsertDate { get;  set; }
        public string Image { get;  set; }
        public int QuantityStock { get;  set; }       
        
    }

    public class Dimension
    {
        public decimal Height { get; }
        public decimal Width { get;  set; }
        public decimal Depth { get;  set; }
    }
}
