using Infrastructure.Core;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Write.User
{
    public interface IUserWriteRepository
    {
        Task<UserWriteDto> SingleOrDefault(string email, CancellationToken cancellationToken = default(CancellationToken));
        Task Add(Domain.User User, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(Domain.User User, CancellationToken cancellationToken = default(CancellationToken));
    }
    public class UserWriteRepository : IUserWriteRepository
    {
        private static string UsersCollection => "Users";

        private readonly IUserWriteMapper _UserWriteMapper;

        private readonly IMongoCollection<UserWriteDto> _dbContext;
        public UserWriteRepository(IUserWriteMapper UserWriteMapper, IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _UserWriteMapper = UserWriteMapper;
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<UserWriteDto>(UsersCollection);
        }

        public async Task<UserWriteDto> SingleOrDefault(string email, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserWriteDto>.Filter.Eq("Email", email);
            return await _dbContext.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Add(Domain.User User, CancellationToken cancellationToken = default)
        {
            var userDto = _UserWriteMapper.ToUserDto(User);
            cancellationToken.ThrowIfCancellationRequested();

            await _dbContext.InsertOneAsync(userDto);
        }

        public async Task Update(Domain.User User, CancellationToken cancellationToken = default)
        {
            var userDto = _UserWriteMapper.ToUserDto(User);
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<UserWriteDto>.Filter.Eq("Email", userDto.Email);
            var update = Builders<UserWriteDto>.Update.Set("Name", userDto.Name)
                                                      .Set("Surname", userDto.Surname)
                                                      .Set("ExternalToken", userDto.ExternalToken);

            await _dbContext.UpdateOneAsync(filter, update);
        }
    }
}
