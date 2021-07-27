using Config;
using MongoDB.Driver;

namespace Infrastructure.Core
{
    public interface IMongoDbConnectionFactory
    {
        IMongoDatabase Connection { get; }
    }

    public class MongoDbConnectionFactory : IMongoDbConnectionFactory
    {
        private readonly MongoDBConnectionString _mongoDBConnectionString;
        private readonly Database _database;

        private IMongoDatabase _mongoDatabase;
        public MongoDbConnectionFactory(MongoDBConnectionString mongoDBConnectionString, Database database)
        {
            _mongoDBConnectionString = mongoDBConnectionString;
            _database = database;
        }

        public IMongoDatabase Connection
        {
            get
            {
                if (_mongoDatabase != null) return _mongoDatabase;

                var client = new MongoClient(_mongoDBConnectionString);
                _mongoDatabase = client.GetDatabase(_database.Name);

                return _mongoDatabase;
            }
        }
    }
}
