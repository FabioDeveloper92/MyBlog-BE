using Infrastructure.Core;
using System;

namespace Infrastructure.Read.Post
{
    public class PostMyOverviewReadDto : Dto
    {
        public string Title { get; }
        public DateTime CreateDate { get; }
        public DateTime? PublishDate { get; }
        
        public PostMyOverviewReadDto(Guid id, string title, DateTime createDate, DateTime? publishDate) : base(id)
        {
            Title = title;
            CreateDate = createDate;
            PublishDate = publishDate;
        }

    }
}
