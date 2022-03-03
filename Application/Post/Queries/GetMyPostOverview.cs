using Application.Interfaces;
using Infrastructure.Core.Enum;
using Infrastructure.Read.Post;
using System.Collections.Generic;

namespace Application.Post.Queries
{
    public class GetMyPostOverview : IQuery<List<PostMyOverviewReadDto>>
    {
        public string UserEmail { get; }
        public string Title { get; }
        public FilterPostStatus Status { get; }
        public OrderPostDate OrderBy { get; }
        public int Limit { get; }

        public GetMyPostOverview(string userEmail, string title, FilterPostStatus status, OrderPostDate orderBy, int limit)
        {
            UserEmail = userEmail;
            Title = title;
            Status = status;
            OrderBy = orderBy;
            Limit = limit;
        }
    }
}
