using System;

namespace KSE.Catalog.Extensions.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() : base() { }

        public DomainException(string exception) : base(exception) { }
    }
}
