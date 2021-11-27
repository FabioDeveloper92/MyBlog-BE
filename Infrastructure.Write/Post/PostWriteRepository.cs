using Infrastructure.Core;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Write.Post
{
    public interface IPostWriteRepository
    {
       Task<Domain.Post> SingleOrDefault(Guid id, CancellationToken cancellationToken = default(CancellationToken));
       Task Add(Domain.Post entity, CancellationToken cancellationToken = default(CancellationToken));
       Task Update(Domain.Post entity, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class PostWriteRepository : IPostWriteRepository
    {
        private static string PostsCollection => "Posts";

        private readonly IPostWriteMapper _postWriteMapper;
        private readonly IMongoCollection<PostWriteDto> _dbContext;
        public PostWriteRepository(IPostWriteMapper taskWriteMapper, IMongoDbConnectionFactory mongoDbConnectionFactory)
        {
            _postWriteMapper = taskWriteMapper;
            _dbContext = mongoDbConnectionFactory.Connection.GetCollection<PostWriteDto>(PostsCollection);
        }

        public async Task<Domain.Post> SingleOrDefault(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<PostWriteDto>.Filter.Eq("_id", id);
            var postWriteDto = await _dbContext.Find(filter).FirstOrDefaultAsync();

            if (postWriteDto == null)
                return null;

            return Domain.Post.Create(postWriteDto.Title, postWriteDto.ImageThumb, postWriteDto.ImageMain, postWriteDto.Text, postWriteDto.Tags, postWriteDto.CreateBy, postWriteDto.CreateDate, postWriteDto.UpdateDate, postWriteDto.PublishDate);
        }

        public async Task Add(Domain.Post entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var postDto = _postWriteMapper.ToPostDto(entity);

            cancellationToken.ThrowIfCancellationRequested();
            await _dbContext.InsertOneAsync(postDto);
        }

        public async Task Update(Domain.Post entity, CancellationToken cancellationToken = default)
        {
            var postDto = _postWriteMapper.ToPostDto(entity);
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<PostWriteDto>.Filter.Eq("_id", postDto.Id);
            var update = Builders<PostWriteDto>.Update.Set("Title", postDto.Title)
                                                      .Set("Text", postDto.Text)
                                                      .Set("ImageMain", postDto.ImageMain)
                                                      .Set("ImageThumb", postDto.ImageThumb)
                                                      .Set("Tags", postDto.Tags)
                                                      .Set("UpdateDate", postDto.UpdateDate)
                                                      .Set("PublishDate", postDto.PublishDate);

            await _dbContext.UpdateOneAsync(filter, update);
        }
    }
}
