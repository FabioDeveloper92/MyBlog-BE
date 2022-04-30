using Infrastructure.Core;
using Infrastructure.Core.Enum;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Read.Post
{
    public interface IPostReadRepository
    {
        Task<PostPublishedReadDto> GetPostPublished(Guid id);
        Task<List<PostOverviewReadDto>> GetAllOverview(int maxItems, FilterByTime filterByTime, OrderByVisibility orderByVisibility);
        Task<PostUpdateReadDto> GetPostAllFields(Guid id);
        Task<List<PostMyOverviewReadDto>> GetMyPosts(string userEmail, string title, FilterPostStatus filterStatus, OrderPostDate orderPost, int limit);
        Task<List<MyPostRelatedSimpleDto>> GetMyPostRelatedSimple(string userEmail);
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

        public async Task<List<PostOverviewReadDto>> GetAllOverview(int maxItems, FilterByTime filterByTime, OrderByVisibility orderByVisibility)
        {
            var commentProjectCheckIsNull = new BsonDocument
            {
                {
                   "$cond", new BsonArray()
                   {
                       new BsonDocument{ { "$gt", new BsonArray() { "$Comments", BsonNull.Value } } },
                       new BsonDocument{ {"$size", "$Comments" } },
                       0
                   }
                }
            };
            var projectBson = new BsonDocument
            {
                { "_id", 1},
                {"Title", 1 },
                {"ImageThumb", 1 },
                {"Tags", 1 },
                {"CreateBy", 1 },
                {"PublishDate", 1 },
                {"CommentNumber", commentProjectCheckIsNull}
            };

            BsonDocument sortBson;
            switch (orderByVisibility)
            {
                case OrderByVisibility.Top:
                    {
                        sortBson = new BsonDocument("PublishDate", -1);
                        break;
                    }
                case OrderByVisibility.Relevant:
                    {
                        sortBson = new BsonDocument {
                            {"CommentNumber", -1},
                            {"PublishDate", -1 },
                        };
                        break;
                    }
                case OrderByVisibility.Latest:
                case OrderByVisibility.Undefined:
                default:
                    {
                        sortBson = new BsonDocument("PublishDate", -1);
                        break;
                    }
            }

            BsonDocument matchBson = null;
            switch (filterByTime)
            {
                case FilterByTime.Week:
                    {
                        matchBson = new BsonDocument
                        {
                            {
                               "$and", new BsonArray()
                                        {
                                            new BsonDocument("PublishDate", new BsonDocument() { { "$ne", BsonNull.Value } }),
                                            new BsonDocument("PublishDate", new BsonDocument() { { "$gte", DateTime.SpecifyKind( DateTime.Now.AddDays(-7),DateTimeKind.Utc) } }),
                                            new BsonDocument("PublishDate", new BsonDocument() { { "$lte", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc) } })
                                        }
                            }
                        };
                        break;
                    }
                case FilterByTime.Month:
                    {
                        matchBson = new BsonDocument
                        {
                            {
                               "$and", new BsonArray()
                                        {
                                            new BsonDocument("PublishDate", new BsonDocument() { { "$ne", BsonNull.Value } }),
                                            new BsonDocument("PublishDate", new BsonDocument() { { "$gte", DateTime.SpecifyKind( DateTime.Now.AddMonths(-1),DateTimeKind.Utc) } }),
                                            new BsonDocument("PublishDate", new BsonDocument() { { "$lte", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc) } })
                                        }
                            }
                        };
                        break;
                    }
                case FilterByTime.Infinity:
                case FilterByTime.Undefined:
                default:
                    {
                        matchBson = new BsonDocument("PublishDate", new BsonDocument() { { "$ne", BsonNull.Value } });
                        break;
                    }
            }

            var pipeline = new BsonDocument[] {
                new BsonDocument{ { "$match", matchBson } },
                new BsonDocument{ { "$project", projectBson } },
                new BsonDocument{ { "$sort",  sortBson }},
                new BsonDocument{ { "$limit", maxItems } }
            };

            return await _dbContext.Aggregate<PostOverviewReadDto>(pipeline).ToListAsync();
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
                                                     .Include("PublishDate")
                                                     .Include("PostsRelated");

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

        public async Task<List<MyPostRelatedSimpleDto>> GetMyPostRelatedSimple(string userEmail)
        {
            var filterPublishDateIsNull = Builders<PostReadMapper>.Filter.Eq("PublishDate", BsonNull.Value);
            var filteruserEmail = Builders<PostReadMapper>.Filter.Eq("CreateBy", userEmail);
            var filter = Builders<PostReadMapper>.Filter.And(filteruserEmail, !filterPublishDateIsNull);

            var projection = Builders<PostReadMapper>.Projection
                                                     .Include("Title");

            var sort = Builders<PostReadMapper>.Sort.Descending("Title");

            return await _dbContext.Find(filter)
                                   .Project<MyPostRelatedSimpleDto>(projection)
                                   .Sort(sort)
                                   .ToListAsync();
        }
    }
}
