namespace Domain.Exceptions
{
    public class UserNotExistException : DomainException
    {
        public UserNotExistException() : base("ERROR.USER-NOT-EXIST-EXCEPTION")
        {
        }
    }
}
