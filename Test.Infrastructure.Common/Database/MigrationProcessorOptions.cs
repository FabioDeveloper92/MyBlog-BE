namespace Test.Infrastructure.Common.Database
{
    internal class MigrationProcessorOptions : IMigrationProcessorOptions
    {
        public bool PreviewOnly { get; set; }
        public int Timeout { get; set; }
    }
}