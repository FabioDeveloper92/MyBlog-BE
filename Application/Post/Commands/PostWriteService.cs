using Domain.Exceptions;
using Infrastructure.Write.Post;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class PostWriteService : IRequestHandler<CreatePost>,
                                    IRequestHandler<UpdatePost>,
                                    IRequestHandler<AddPostComment>,
                                    IRequestHandler<AddPostRelated>
    {
        private readonly IPostWriteRepository _postWriteRepository;

        public PostWriteService(IPostWriteRepository postWriteRepository)
        {
            _postWriteRepository = postWriteRepository;
        }

        public async Task<Unit> Handle(CreatePost command, CancellationToken cancellationToken)
        {
            var entity = Domain.Post.Create(command.Title, command.ImageThumb, command.ImageMain, command.Text, command.Tags, command.CreateBy, command.CreateDate, command.UpdateDate, command.PublishDate, null, command.PostsRelated, command.Id);

            await _postWriteRepository.Add(entity);

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdatePost command, CancellationToken cancellationToken)
        {
           var entity = await _postWriteRepository.SingleOrDefault(command.Id);

            if (entity == null)
                throw new PostNotFoundException();

            if (entity.PublishDate.HasValue)
                throw new OperationNotAvailableException();

            entity.SetTitle(command.Title);
            entity.SetImageThumb(command.ImageThumb);
            entity.SetImageMain(command.ImageMain);
            entity.SetTags(command.Tags);
            entity.SetCreateBy(command.CreateBy);
            entity.SetUpdateDate(command.UpdateDate);
            entity.SetPublishDate(command.PublishDate);
            entity.SetPostsRelated(command.PostsRelated);

            await _postWriteRepository.Update(entity);

            return Unit.Value;
        }

        public async Task<Unit> Handle(AddPostComment command, CancellationToken cancellationToken)
        {
            var entity = await _postWriteRepository.SingleOrDefault(command.PostId);

            if (entity == null)
                throw new PostNotFoundException();

            entity.AddComment(command.Username, command.Text, command.CreateDate, command.Id);

            await _postWriteRepository.Update(entity);

            return Unit.Value;
        }  
        
        public async Task<Unit> Handle(AddPostRelated command, CancellationToken cancellationToken)
        {
            var entity = await _postWriteRepository.SingleOrDefault(command.PostId);

            if (entity == null)
                throw new PostNotFoundException();

            entity.AddPostRelated(command.PostRelatedId);

            await _postWriteRepository.Update(entity);

            return Unit.Value;
        }
    }
}
