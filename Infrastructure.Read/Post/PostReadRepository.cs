using Infrastructure.Core;
using MongoDB.Bson;
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
        Task<List<PostOverviewReadDto>> GetAllOverview(int maxItems);
    }
    public class PostReadRepository : IPostReadRepository
    {
        private static string PostsCollection => "Posts";

        private readonly IMongoCollection<PostReadMapper> _dbContext;
        public PostReadRepository(IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<PostReadMapper>(PostsCollection);
        }

        public async Task<PostReadDto> SingleOrDefault(Guid id)
        {
            var filterId = Builders<PostReadMapper>.Filter.Eq("_id", id);
            var filterPublishDate = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);

            var postReadMapper = await _dbContext.Find(filterId & !filterPublishDate).FirstOrDefaultAsync();

            if (postReadMapper == null)
                return null;

            return postReadMapper.toPostReadDto();
        }

        public async Task<List<PostReadDto>> GetAll()
        {
            var filterPublishDateIsNull = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);

            var postsReadMapper = await _dbContext.Find(!filterPublishDateIsNull).ToListAsync();

            if (postsReadMapper == null)
                return null;

            var res = new List<PostReadDto>();

            foreach (var postReadMapper in postsReadMapper)
            {
                res.Add(postReadMapper.toPostReadDto());
            }

            return res;
        }

        public async Task<List<PostOverviewReadDto>> GetAllOverview(int maxItems)
        {
            var filterPublishDateIsNull = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);

            var postsReadMapper = await _dbContext.Find(!filterPublishDateIsNull)
                                                  .SortByDescending(p => p.PublishDate)
                                                  .Limit(maxItems)
                                                  .ToListAsync();

            if (postsReadMapper == null)
                return null;

            var res = new List<PostOverviewReadDto>();

            foreach (var postReadMapper in postsReadMapper)
            {
                res.Add(postReadMapper.toPostOverViewReadDto());
            }

            return res;
        }
    }
}
