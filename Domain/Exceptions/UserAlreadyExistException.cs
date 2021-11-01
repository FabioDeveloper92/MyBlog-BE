namespace Domain.Exceptions
{
    public class UserAlreadyExistException : DomainException
    {
        public UserAlreadyExistException() : base("ERROR.USER-ALREADY-EXIST-EXCEPTION")
        {
        }
    }
}
