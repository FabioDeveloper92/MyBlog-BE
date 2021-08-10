using Infrastructure.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Read.Post
{
    public interface IPostReadRepository
    {
        Task<PostReadDto> SingleOrDefault(Guid id);
        Task<List<PostReadDto>> GetAll();
    }
    public class PostReadRepository : IPostReadRepository
    {
        private static string PostsCollection => "Posts";

        private readonly IMongoCollection<PostReadDto> _dbContext;
        public PostReadRepository(IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<PostReadDto>(PostsCollection);
        }

        public async Task<PostReadDto> SingleOrDefault(Guid id)
        {
            var filter = Builders<PostReadDto>.Filter.Eq("_id", id);
            return await _dbContext.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<PostReadDto>> GetAll()
        {
            return await _dbContext.Find(_ => true).ToListAsync();
        }
    }
}
