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
        Task<PostUpdateReadDto> GetPostAllFields(Guid id);
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

            var projection = Builders<PostReadMapper>.Projection
                                                     .Include("Title")
                                                     .Include("Text")
                                                     .Include("ImageMain")
                                                     .Include("Tags")
                                                     .Include("CreateBy")
                                                     .Include("PublishDate");

            return await _dbContext.Find(filterId & !filterPublishDate).Project<PostReadDto>(projection).FirstOrDefaultAsync();
        }

        public async Task<List<PostReadDto>> GetAll()
        {
            var filterPublishDateIsNull = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);

            var projection = Builders<PostReadMapper>.Projection
                                                     .Include("Title")
                                                     .Include("Text")
                                                     .Include("ImageMain")
                                                     .Include("Tags")
                                                     .Include("CreateBy")
                                                     .Include("PublishDate");

            return await _dbContext.Find(!filterPublishDateIsNull).Project<PostReadDto>(projection).ToListAsync();
        }

        public async Task<List<PostOverviewReadDto>> GetAllOverview(int maxItems)
        {
            var filterPublishDateIsNull = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);

            var projection = Builders<PostReadMapper>.Projection
                                                     .Include("Title")
                                                     .Include("ImageThumb")
                                                     .Include("Tags")
                                                     .Include("CreateBy")
                                                     .Include("PublishDate");

            return await _dbContext.Find(!filterPublishDateIsNull)
                                    .Project<PostOverviewReadDto>(projection)
                                    .SortByDescending(p => p.PublishDate)
                                    .Limit(maxItems)
                                    .ToListAsync();
        }

        public async Task<PostUpdateReadDto> GetPostAllFields(Guid id)
        {
            var filterId = Builders<PostReadMapper>.Filter.Eq("_id", id);

            var projection = Builders<PostReadMapper>.Projection
                                                     .Include("Title")
                                                     .Include("ImageThumb")
                                                     .Include("ImageMain")
                                                     .Include("Text")
                                                     .Include("Tags")
                                                     .Include("CreateBy")
                                                     .Include("CreateDate")
                                                     .Include("UpdateDate")
                                                     .Include("PublishDate");

            return await _dbContext.Find(filterId).Project<PostUpdateReadDto>(projection).FirstOrDefaultAsync();
        }
    }
}
