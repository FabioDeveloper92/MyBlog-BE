namespace Domain.Exceptions
{
    public class NotFoundItemException : DomainException
    {
        public NotFoundItemException() : base("ERROR.ITEM-FOUND-EXCEPTION")
        {
        }
    }
}
