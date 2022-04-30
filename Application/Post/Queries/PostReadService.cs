using Domain.Exceptions;
using Infrastructure.Read.Post;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Post.Queries
{
    public class PostReadService : IRequestHandler<GetPostPublished, PostPublishedReadDto>,
                                   IRequestHandler<GetPostsOverview, List<PostOverviewReadDto>>,
                                   IRequestHandler<GetPostUpdate, PostUpdateReadDto>,
                                   IRequestHandler<GetMyPostOverview, List<PostMyOverviewReadDto>>,
                                   IRequestHandler<GetMyPostRelatedSimple, List<MyPostRelatedSimpleDto>>

    {
        private readonly IPostReadRepository _postReadRepository;

        public PostReadService(IPostReadRepository postReadRepository)
        {
            _postReadRepository = postReadRepository;
        }

        public async Task<PostPublishedReadDto> Handle(GetPostPublished request, CancellationToken cancellationToken)
        {
            var post = await _postReadRepository.GetPostPublished(request.Id);

            if (post == null)
                throw new PostNotFoundException();

            return post;
        }

        public async Task<List<PostOverviewReadDto>> Handle(GetPostsOverview request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.GetAllOverview(request.MaxItems, request.FilterByTime, request.OrderByVisibility);
        }

        public async Task<PostUpdateReadDto> Handle(GetPostUpdate request, CancellationToken cancellationToken)
        {
            var post = await _postReadRepository.GetPostAllFields(request.Id);
            if (post == null)
                throw new PostNotFoundException();

            return post;
        }

        public async Task<List<PostMyOverviewReadDto>> Handle(GetMyPostOverview request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.GetMyPosts(request.UserEmail, request.Title, request.Status, request.OrderBy, request.Limit);
        }

        public async Task<List<MyPostRelatedSimpleDto>> Handle(GetMyPostRelatedSimple request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.GetMyPostRelatedSimple(request.UserMail);
        }
    }
}
