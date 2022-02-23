using Domain.Exceptions;
using Infrastructure.Read.Post;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Post.Queries
{
    public class PostReadService : IRequestHandler<GetPost, PostReadDto>,
                                   IRequestHandler<GetPosts, List<PostReadDto>>,
                                   IRequestHandler<GetPostsOverview, List<PostOverviewReadDto>>,
                                   IRequestHandler<GetPostUpdate, PostUpdateReadDto>,
                                   IRequestHandler<GetMyPostOverview, List<PostMyOverviewReadDto>>

    {
        private readonly IPostReadRepository _postReadRepository;

        public PostReadService(IPostReadRepository postReadRepository)
        {
            _postReadRepository = postReadRepository;
        }
        public async Task<PostReadDto> Handle(GetPost request, CancellationToken cancellationToken)
        {
            var post = await _postReadRepository.SingleOrDefault(request.Id);

            if (post == null)
                throw new PostNotFoundException();

            return post;
        }

        public async Task<List<PostReadDto>> Handle(GetPosts request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.GetAll();
        }

        public async Task<List<PostOverviewReadDto>> Handle(GetPostsOverview request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.GetAllOverview(request.MaxItems);
        }

        public async Task<PostUpdateReadDto> Handle(GetPostUpdate request, CancellationToken cancellationToken)
        {
            var post= await _postReadRepository.GetPostAllFields(request.Id);
            if (post == null)
                throw new PostNotFoundException();

            return post;
        }

        public async Task<List<PostMyOverviewReadDto>> Handle(GetMyPostOverview request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.GetMyPosts(request.Title, request.Status, request.OrderBy, request.Limit);
        }
    }
}
