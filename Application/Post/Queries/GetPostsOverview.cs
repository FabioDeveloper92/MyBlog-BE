using Application.Interfaces;
using Infrastructure.Core.Enum;
using Infrastructure.Read.Post;
using System.Collections.Generic;

namespace Application.Post.Queries
{

    public class GetPostsOverview : IQuery<List<PostOverviewReadDto>>
    {
        public int MaxItems { get; }
        public FilterByTime FilterByTime { get; }
        public OrderByVisibility OrderByVisibility { get; }

        public GetPostsOverview(int numItems, FilterByTime filterByTime, OrderByVisibility orderByVisibility)
        {
            MaxItems = numItems;
            FilterByTime = filterByTime;
            OrderByVisibility = orderByVisibility;
        }
    }
}
