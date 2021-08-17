using Infrastructure.Core;
using Infrastructure.Write.Post;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class PostWriteService : IRequestHandler<CreatePost>
    {
        private readonly IPostWriteRepository _postWriteRepository;

        public PostWriteService(IPostWriteRepository postWriteRepository)
        {
            _postWriteRepository = postWriteRepository;
        }

        public async Task<Unit> Handle(CreatePost command, CancellationToken cancellationToken)
        {
            var entity = Domain.Post.Create(command.Title, command.Text, command.Category, command.ImageUrl, command.CreateDate, command.CreateBy, command.PostId);

            await _postWriteRepository.Add(entity);

            return Unit.Value;
        }
    }
}
