using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Infrastructure.Common.Database
{
    public class FluentDbAssertion
    {
        private readonly IMongoDatabase _connection;

        public FluentDbAssertion(IMongoDatabase connection)
        {
            _connection = connection;
        }
        public async Task ShouldExists<T>(string table, params Guid[] ids)
        {
            var count = await Count<T>(table, ids);
            count.Should().Be(ids.Length);
        }

        public async Task ShouldNotExists<T>(string table, params Guid[] ids)
        {
            var count = await Count<T>(table, ids);
            count.Should().Be(0);
        }
        private async Task<int> Count<T>(string collectionName, Guid[] ids)
        {
            var objIds = ids.Select(x => new ObjectId(x.ToString()));
            var filterStr = "{_id: {$in: " + objIds + "}}";

            var dbContext = _connection.GetCollection<T>(collectionName);
            return (int)await dbContext.CountDocumentsAsync(filterStr);
        }
    }
}
