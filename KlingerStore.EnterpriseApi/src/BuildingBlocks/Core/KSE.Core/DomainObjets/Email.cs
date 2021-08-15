using KSE.Core.Tools;

namespace KSE.Core.DomainObjets
{
    public class Email
    {
        public const int MaxLength = 254;
        public const int MinLength = 5;
        public string EmailAddress { get; private set; }        
        protected Email() { }
        public Email(string emailAddress)
        {
            if (!ValidationMethods.IsEmail(emailAddress)) throw new DomainException("E-mail inválido");
            EmailAddress = emailAddress;
        }
    }
}