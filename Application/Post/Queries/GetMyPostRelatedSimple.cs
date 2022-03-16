using Application.Interfaces;
using Infrastructure.Read.Post;
using System.Collections.Generic;

namespace Application.Post.Queries
{
    public class GetMyPostRelatedSimple : IQuery<List<MyPostRelatedSimpleDto>>
    {
        public string UserMail { get; }

        public GetMyPostRelatedSimple(string userMail)
        {
            UserMail = userMail;
        }
    }
}
