using Application.Interfaces;
using Infrastructure.Read.Post;
using System.Collections.Generic;

namespace Application.Post.Queries
{

    public class GetPostsOverview : IQuery<List<PostOverviewReadDto>>
    {
        public int MaxItems { get; }

        public GetPostsOverview(int numItems)
        {
            MaxItems = numItems;
        }
    }
}
