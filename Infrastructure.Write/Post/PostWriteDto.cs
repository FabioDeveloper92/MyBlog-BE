using Domain;
using Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace Infrastructure.Write.Post
{
    public class PostWriteDto : Dto
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
        public List<PostComment> Comments { get; }

        public PostWriteDto(Guid id, string title, string imageThumb, string imageMain, string text, int[] tags, string createBy, DateTime createDate, DateTime updateDate, DateTime? publishDate, List<PostComment> comments) : base(id)
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
            Comments = comments;
        }
    }
}
