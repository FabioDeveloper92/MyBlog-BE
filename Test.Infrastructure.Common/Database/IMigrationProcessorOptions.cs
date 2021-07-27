namespace Test.Infrastructure.Common.Database
{
    internal interface IMigrationProcessorOptions
    {
        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}