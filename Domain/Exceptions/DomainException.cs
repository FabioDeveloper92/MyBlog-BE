using System;

namespace Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string Code { get; }
        public string Field { get; }

        public DomainException(string code, string field = null)
        {
            Code = code;
            Field = field;
        }
    }
}
