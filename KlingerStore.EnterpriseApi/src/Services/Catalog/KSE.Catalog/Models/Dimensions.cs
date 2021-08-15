
namespace KSE.Catalog.Models
{
    public class Dimensions
    {
        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }

        public Dimensions(decimal height, decimal width, decimal depth)
        {
            Height = height;
            Width = width;
            Depth = depth;

            Validite();
        }
        public string DescriptionFormat()
        {
            return $"LxAxP: {Height} x {Width} x {Depth}";
        }
        public override string ToString()
        {
            return DescriptionFormat();
        }
        public void Validite()
        {        
            /*
            Validation.ValidateIfLessThan(Height, 1, "O campo Altura não pode ser menor ou igual a 0");
            Validation.ValidateIfLessThan(Width, 1, "O campo Largura não pode ser menor ou igual a 0");
            Validation.ValidateIfLessThan(Depth, 1, "O campo Profundidade não pode ser menor ou igual a 0");
            */
        }
    }
}
