namespace Application.Code
{
    public interface IContextProvider
    {
        string TryGetName();
        string TryGetEmail();
        string BuildUrl(string relativeUrl = null);
    }
}
