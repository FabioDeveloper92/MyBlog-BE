using Infrastructure.Core;
using Infrastructure.Core.Enum;
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
        Task<List<PostMyOverviewReadDto>> GetMyPosts(string title, FilterPostStatus filterStatus, OrderPostDate orderPost, int limit);
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

        public async Task<List<PostMyOverviewReadDto>> GetMyPosts(string title, FilterPostStatus filterStatus, OrderPostDate orderPost, int limit)
        {
            var filterTitleContains = Builders<PostReadMapper>.Filter.Regex("Title", $".*{title}.*");

            FilterDefinition<PostReadMapper> filterByStatus = null;
            if (filterStatus == FilterPostStatus.Draft)
            {
                filterByStatus = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);
            }
            else if (filterStatus == FilterPostStatus.Published)
            {
                filterByStatus = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);
            }

            FilterDefinition<PostReadMapper> filter;
            if (filterByStatus != null)
            {
                filter = Builders<PostReadMapper>.Filter.And(filterTitleContains, filterByStatus);
            }
            else
            {
                filter = filterTitleContains;
            }

            var projection = Builders<PostReadMapper>.Projection
                                         .Include("Title")
                                         .Include("CreateDate")
                                         .Include("PublishDate");

            var sort = Builders<PostReadMapper>.Sort.Ascending("CreateDate");
            if (orderPost == OrderPostDate.RecentlyPublished)
                sort = Builders<PostReadMapper>.Sort.Ascending("PublishDate");

            return await _dbContext.Find(filter)
                                   .Project<PostMyOverviewReadDto>(projection)
                                   .Sort(sort)
                                   .Limit(limit)
                                   .ToListAsync();
        }
    }
}
