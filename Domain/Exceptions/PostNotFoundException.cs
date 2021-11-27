namespace Domain.Exceptions
{
    public class PostNotFoundException : DomainException
    {
        public PostNotFoundException() : base("ERROR.POST-NOT-FOUND-EXCEPTION")
        {
        }
    }
}
