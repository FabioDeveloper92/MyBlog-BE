using Application.Interfaces;
using Infrastructure.Read.Post;
using System.Collections.Generic;

namespace Application.Post.Queries
{
    public class GetPosts : IQuery<List<PostReadDto>>
    {
    }
}
