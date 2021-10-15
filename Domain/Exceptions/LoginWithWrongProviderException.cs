namespace Domain.Exceptions
{
    public class LoginWithWrongProviderException : DomainException
    {
        public LoginWithWrongProviderException() : base("ERROR.LOGIN-WITH-WRONG-PROVIDER")
        {

        }
    }
}
