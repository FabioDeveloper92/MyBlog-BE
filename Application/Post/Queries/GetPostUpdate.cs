using Application.Interfaces;
using Infrastructure.Read.Post;
using System;

namespace Application.Post.Queries
{
    public class GetPostUpdate : IQuery<PostUpdateReadDto>
    {
        public Guid Id { get; }

        public GetPostUpdate(Guid id)
        {
            Id = id;
        }
    }
}
