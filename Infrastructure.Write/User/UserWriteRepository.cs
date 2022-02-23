using Infrastructure.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Write.User
{
    public interface IUserWriteRepository
    {
        Task<Domain.User> SingleOrDefault(string email, CancellationToken cancellationToken = default(CancellationToken));
        Task Add(Domain.User User, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(Domain.User User, CancellationToken cancellationToken = default(CancellationToken));
    }
    public class UserWriteRepository : IUserWriteRepository
    {
        private static string UsersCollection => "Users";

        private readonly IUserWriteMapper _userWriteMapper;

        private readonly IMongoCollection<UserWriteDto> _dbContext;
        public UserWriteRepository(IUserWriteMapper UserWriteMapper, IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _userWriteMapper = UserWriteMapper;
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<UserWriteDto>(UsersCollection);
        }

        public async Task<Domain.User> SingleOrDefault(string email, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserWriteDto>.Filter.Eq("Email", email);
            var userWriteDto = await _dbContext.Find(filter).FirstOrDefaultAsync();

            if (userWriteDto == null)
                return null;

            return Domain.User.Create(userWriteDto.Name, userWriteDto.Surname, userWriteDto.Email, userWriteDto.Password, userWriteDto.LoginWith, userWriteDto.Id);
        }

        public async Task Add(Domain.User User, CancellationToken cancellationToken = default)
        {
            var userDto = _userWriteMapper.ToUserDto(User);
            cancellationToken.ThrowIfCancellationRequested();

            await _dbContext.InsertOneAsync(userDto);

        }

        public async Task Update(Domain.User User, CancellationToken cancellationToken = default)
        {
            var userDto = _userWriteMapper.ToUserDto(User);
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<UserWriteDto>.Filter.Eq("Email", userDto.Email);
            var update = Builders<UserWriteDto>.Update.Set("Name", userDto.Name)
                                                      .Set("Surname", userDto.Surname);

            await _dbContext.UpdateOneAsync(filter, update);
        }

    }
}
