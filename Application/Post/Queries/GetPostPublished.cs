using Application.Interfaces;
using Infrastructure.Read.Post;
using System;

namespace Application.Post.Queries
{
    public class GetPostPublished : IQuery<PostPublishedReadDto>
    {
        public Guid Id { get; }

        public GetPostPublished(Guid id)
        {
            Id = id;
        }
    }
}
