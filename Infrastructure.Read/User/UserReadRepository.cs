using Infrastructure.Core;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Infrastructure.Read.User
{
    public interface IUserReadRepository
    {
        Task<UserReadDto> SingleOrDefault(string internalToken);
    }
    public class UserReadRepository : IUserReadRepository
    {
        private static string UsersCollection => "Users";

        private readonly IMongoCollection<UserReadDto> _dbContext;
        public UserReadRepository(IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<UserReadDto>(UsersCollection);
        }

        public async Task<UserReadDto> SingleOrDefault(string internalToken)
        {
            var filter = Builders<UserReadDto>.Filter.Eq("InternalToken", internalToken);
            var projection = Builders<UserReadDto>.Projection.Include("Name")
                                                             .Include("Surname")
                                                             .Include("Email")
                                                             .Include("InternalToken")
                                                             .Exclude("_id");

            return await _dbContext.Find(filter).Project<UserReadDto>(projection).FirstOrDefaultAsync();
        }
    }
}
