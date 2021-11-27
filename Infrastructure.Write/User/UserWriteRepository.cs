using Infrastructure.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Write.User
{
    public interface IUserWriteRepository
    {
        Task<Domain.User> SingleOrDefault(string email, CancellationToken cancellationToken = default(CancellationToken));
        Task<Domain.User> SingleOrDefaultByInternalToken(string internalToken, CancellationToken cancellationToken = default(CancellationToken));
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

        public async Task<Domain.User> SingleOrDefault(string email, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserWriteDto>.Filter.Eq("Email", email);
            var userWriteDto = await _dbContext.Find(filter).FirstOrDefaultAsync();

            if (userWriteDto == null)
                return null;

            return Domain.User.Create(userWriteDto.Name, userWriteDto.Surname, userWriteDto.Email, userWriteDto.Password, userWriteDto.ExternalToken, userWriteDto.LoginWith, userWriteDto.InternalToken, userWriteDto.ExpiredToken, userWriteDto.Id);
        }

        public async Task<Domain.User> SingleOrDefaultByInternalToken(string internalToken, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserWriteDto>.Filter.Eq("InternalToken", internalToken);
            var userWriteDto = await _dbContext.Find(filter).FirstOrDefaultAsync();

            if (userWriteDto == null)
                return null;

            return Domain.User.Create(userWriteDto.Name, userWriteDto.Surname, userWriteDto.Email, userWriteDto.Password, userWriteDto.ExternalToken, userWriteDto.LoginWith, userWriteDto.InternalToken, userWriteDto.ExpiredToken, userWriteDto.Id);
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
                                                      .Set("InternalToken", userDto.InternalToken)
                                                      .Set("ExternalToken", userDto.ExternalToken);

            await _dbContext.UpdateOneAsync(filter, update);
        }

    }
}
