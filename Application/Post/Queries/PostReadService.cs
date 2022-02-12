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
                                   IRequestHandler<GetPostUpdate, PostUpdateReadDto>

    {
        private readonly IPostReadRepository _postReadRepository;

        public PostReadService(IPostReadRepository postReadRepository)
        {
            _postReadRepository = postReadRepository;
        }
        public async Task<PostReadDto> Handle(GetPost request, CancellationToken cancellationToken)
        {
            return await _postReadRepository.SingleOrDefault(request.Id);
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
            return await _postReadRepository.GetPostAllFields(request.Id);
        }
    }
}
