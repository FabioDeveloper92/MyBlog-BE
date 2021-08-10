using Infrastructure.Read.Post;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Post.Queries
{
    public class PostReadService : IRequestHandler<GetPost, PostReadDto>,
                                   IRequestHandler<GetPosts, List<PostReadDto>>
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
    }
}
