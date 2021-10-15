namespace Domain.Exceptions
{
    public class InvalidDateException : DomainException
    {
        public InvalidDateException(string field, string value) : base("ERROR.INVALID-DATE")
        {
        }
    }
}
