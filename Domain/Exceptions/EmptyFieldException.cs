namespace Domain.Exceptions
{
    public class EmptyFieldException : DomainException
    {
        public EmptyFieldException(string field) : base("ERROR.EMPTY-FIELD")
        {
        }
    }
}
