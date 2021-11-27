using Domain.Exceptions;
using Infrastructure.Write.Post;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class PostWriteService : IRequestHandler<CreatePost>,
                                    IRequestHandler<UpdatePost>
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

        public async Task<Unit> Handle(UpdatePost command, CancellationToken cancellationToken)
        {
            var entity = await _postWriteRepository.SingleOrDefault(command.Id);

            if (entity == null)
                throw new PostNotFoundException();

            entity.SetTitle(command.Title);
            entity.SetImageThumb(command.ImageThumb);
            entity.SetImageMain(command.ImageMain);
            entity.SetTags(command.Tags);
            entity.SetCreateBy(command.CreateBy);
            entity.SetUpdateDate(command.UpdateDate);
            entity.SetPublishDate(command.PublishDate);

            await _postWriteRepository.Update(entity);

            return Unit.Value;
        }
    }
}
