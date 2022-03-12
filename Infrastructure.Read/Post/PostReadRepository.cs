using Infrastructure.Core;
using Infrastructure.Core.Enum;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Read.Post
{
    public interface IPostReadRepository
    {
        Task<PostPublishedReadDto> GetPostPublished(Guid id);
        Task<List<PostReadDto>> GetAll();
        Task<List<PostOverviewReadDto>> GetAllOverview(int maxItems);
        Task<PostUpdateReadDto> GetPostAllFields(Guid id);
        Task<List<PostMyOverviewReadDto>> GetMyPosts(string userEmail, string title, FilterPostStatus filterStatus, OrderPostDate orderPost, int limit);
    }
    public class PostReadRepository : IPostReadRepository
    {
        private static string PostsCollection => "Posts";

        private readonly IMongoCollection<PostReadMapper> _dbContext;
        public PostReadRepository(IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<PostReadMapper>(PostsCollection);
        }

        public async Task<PostPublishedReadDto> GetPostPublished(Guid id)
        {
            var filterId = Builders<PostReadMapper>.Filter.Eq("_id", id);
            var filterPublishDate = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);

            var projection = Builders<BsonDocument>.Projection
                                                   .Include("Title")
                                                   .Include("Text")
                                                   .Include("ImageMain")
                                                   .Include("Tags")
                                                   .Include("CreateBy")
                                                   .Include("PublishDate")
                                                   .Include("Comments")
                                                   .Include("PostsRelatedCompleted._id")
                                                   .Include("PostsRelatedCompleted.Title")
                                                   .Include("PostsRelatedCompleted.ImageThumb");

            return await _dbContext.Aggregate()
                                    .Match(filterId & !filterPublishDate)
                                    .Lookup(PostsCollection, "PostsRelated", "_id", "PostsRelatedCompleted")
                                    .Project<PostPublishedReadDto>(projection)
                                    .FirstOrDefaultAsync();
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
                                                     .Include("PublishDate")
                                                     .Include("Comments")
                                                     .Include("PostsRelated");

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

        public async Task<List<PostMyOverviewReadDto>> GetMyPosts(string userEmail, string title, FilterPostStatus filterStatus, OrderPostDate orderPost, int limit)
        {
            var queryExpr = new BsonRegularExpression(new Regex(title, RegexOptions.IgnoreCase));

            var filterTitleContains = Builders<PostReadMapper>.Filter.Regex("Title", queryExpr);
            var filteruserEmail = Builders<PostReadMapper>.Filter.Eq("CreateBy", userEmail);

            FilterDefinition<PostReadMapper> filterByStatus = null;
            if (filterStatus == FilterPostStatus.Draft)
            {
                filterByStatus = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);
            }
            else if (filterStatus == FilterPostStatus.Published)
            {
                var filterPublish = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);
                filterByStatus = Builders<PostReadMapper>.Filter.Not(filterPublish);
            }

            FilterDefinition<PostReadMapper> filter;
            if (filterByStatus != null)
            {
                filter = Builders<PostReadMapper>.Filter.And(filteruserEmail, filterTitleContains, filterByStatus);
            }
            else
            {
                filter = Builders<PostReadMapper>.Filter.And(filteruserEmail, filterTitleContains);
            }

            var projection = Builders<PostReadMapper>.Projection
                                                     .Include("Title")
                                                     .Include("CreateDate")
                                                     .Include("PublishDate");

            var sort = Builders<PostReadMapper>.Sort.Descending("CreateDate");
            if (orderPost == OrderPostDate.RecentlyPublished)
                sort = Builders<PostReadMapper>.Sort.Descending("PublishDate");



            return await _dbContext.Find(filter)
                                   .Project<PostMyOverviewReadDto>(projection)
                                   .Sort(sort)
                                   .Limit(limit)
                                   .ToListAsync();
        }
    }
}
