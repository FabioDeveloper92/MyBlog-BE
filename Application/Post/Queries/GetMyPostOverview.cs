using Application.Interfaces;
using Infrastructure.Core.Enum;
using Infrastructure.Read.Post;
using System.Collections.Generic;

namespace Application.Post.Queries
{
    public class GetMyPostOverview : IQuery<List<PostMyOverviewReadDto>>
    {
        public string Title { get; }
        public FilterPostStatus Status { get; }
        public OrderPostDate OrderBy { get; }
        public int Limit { get; }

        public GetMyPostOverview(string title, FilterPostStatus status, OrderPostDate orderBy, int limit)
        {
            Title = title;
            Status = status;
            OrderBy = orderBy;
            Limit = limit;
        }
    }
}
