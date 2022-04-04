using Infrastructure.Core;
using System;

namespace Infrastructure.Read.Post
{
    public class PostOverviewReadDto : Dto
    {
        public string Title { get; }
        public string ImageThumb { get; }
        public int[] Tags { get; }
        public string CreateBy { get; }
        public DateTime? PublishDate { get; }
        public int CommentNumber { get; }

        public PostOverviewReadDto(Guid id, string title, string imageThumb, int[] tags, string createBy, DateTime? publishDate, int commentNumber) : base(id)
        {
            Title = title;
            ImageThumb = imageThumb;
            Tags = tags;
            CreateBy = createBy;
            PublishDate = publishDate;
            CommentNumber = commentNumber;
        }

    }
}
