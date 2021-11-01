namespace Domain.Exceptions
{
    public class PasswordNotValidException : DomainException
    {
        public PasswordNotValidException() : base("ERROR.PASSWORD-EXCEPTION")
        {
        }
    }
}
