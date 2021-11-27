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
            var entity = Domain.Post.Create(command.Title, command.ImageThumb, command.ImageMain, command.Text, command.Tags, command.CreateBy, command.CreateDate, command.UpdateDate, command.PublishDate, command.Id);

            await _postWriteRepository.Add(entity);

            return Unit.Value;
        }
    }
}
