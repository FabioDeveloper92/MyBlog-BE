using Infrastructure.Core;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Write.Post
{
    public interface IPostWriteRepository
    {
       Task Add(Domain.Post postWriteDto, CancellationToken cancellationToken = default(CancellationToken));
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

        public async Task Add(Domain.Post postWriteDto, CancellationToken cancellationToken = default(CancellationToken))
        {
            var postDto = _postWriteMapper.ToPostDto(postWriteDto);

            cancellationToken.ThrowIfCancellationRequested();
            await _dbContext.InsertOneAsync(postDto);
        }
    }
}
