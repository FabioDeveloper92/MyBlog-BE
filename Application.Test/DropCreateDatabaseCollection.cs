using Test.Infrastructure.Common.Database;
using Xunit;

namespace Application.Test
{
    [CollectionDefinition("DropCreateDatabase Collection")]
    public class DropCreateDatabaseCollection : ICollectionFixture<DropCreateDatabase>
    {
    }
}
