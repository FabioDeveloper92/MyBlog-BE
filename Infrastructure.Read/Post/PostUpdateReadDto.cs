using Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace Infrastructure.Read.Post
{
    public class PostUpdateReadDto : Dto
    {
        public string Title { get; }
        public string ImageThumb { get; }
        public string ImageMain { get; }
        public string Text { get; }
        public int[] Tags { get; }
        public string CreateBy { get; }
        public DateTime CreateDate { get; }
        public DateTime UpdateDate { get; }
        public DateTime? PublishDate { get; }
        public List<Guid> PostsRelated { get; }

        public PostUpdateReadDto(Guid id, string title, string imageThumb, string imageMain, string text, int[] tags, string createBy, DateTime createDate, DateTime updateDate, DateTime? publishDate, List<Guid> postsRelated) : base(id)
        {
            Title = title;
            ImageThumb = imageThumb;
            ImageMain = imageMain;
            Text = text;
            Tags = tags;
            CreateBy = createBy;
            CreateDate = createDate;
            UpdateDate = updateDate;
            PublishDate = publishDate;
            PostsRelated = postsRelated;
        }

    }
}
