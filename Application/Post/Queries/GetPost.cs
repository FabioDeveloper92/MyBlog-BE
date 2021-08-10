using Application.Interfaces;
using Infrastructure.Read.Post;
using System;

namespace Application.Post.Queries
{
    public class GetPost : IQuery<PostReadDto>
    {
        public Guid Id { get; }

        public GetPost(Guid id)
        {
            Id = id;
        }
    }
}
