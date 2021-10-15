namespace Domain.Exceptions
{
    public class LoginProviderNotExistException : DomainException
    {
        public LoginProviderNotExistException(string field) : base("ERROR.PROVIDER-LOGIN-NOT-EXIST-EXCEPTION")
        {
        }
    }
}
