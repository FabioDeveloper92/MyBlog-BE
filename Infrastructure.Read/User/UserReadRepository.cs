using Infrastructure.Core;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Read.User
{
    public interface IUserReadRepository
    {
        Task<UserReadDto> SingleOrDefault(string email);
    }
    public class UserReadRepository : IUserReadRepository
    {
        private static string UsersCollection => "Users";

        private readonly IMongoCollection<UserReadDto> _dbContext;
        public UserReadRepository(IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<UserReadDto>(UsersCollection);
        }

        public async Task<UserReadDto> SingleOrDefault(string email)
        {
            var filter = Builders<UserReadDto>.Filter.Eq("Email", email);

            var projection = Builders<UserReadDto>.Projection.Include("Name")
                                                             .Include("Surname")
                                                             .Include("Email");

            return await _dbContext.Find(filter).Project<UserReadDto>(projection).FirstOrDefaultAsync();
        }
    }
}
