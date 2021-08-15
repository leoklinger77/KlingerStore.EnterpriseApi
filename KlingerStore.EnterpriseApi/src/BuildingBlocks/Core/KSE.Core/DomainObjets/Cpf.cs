using KSE.Core.Tools;

namespace KSE.Core.DomainObjets
{
    public class Cpf
    {
        public const int CpfMaxLength = 11;
        public string Numero { get; private set; }        
        protected Cpf() { }

        public Cpf(string numero)
        {
            if (!ValidationMethods.IsCpf(numero)) throw new DomainException("CPF inválido");
            Numero = numero;
        }
    }
}