using Infrastructure.Core;
using System;

namespace Infrastructure.Read.Post
{
    public class PostReadDto : Dto
    {
        public string Title { get; }
        public string Text { get; }
        public string ImageMain { get; }
        public int[] Tags { get; }
        public string CreateBy { get; }
        public DateTime? PublishDate { get; }
        
        public PostReadDto(Guid id, string title, string text, string imageMain, int[] tags, string createBy, DateTime? publishDate) : base(id)
        {
            Title = title;
            Text = text;
            ImageMain = imageMain;
            Tags = tags;
            CreateBy = createBy;
            PublishDate = publishDate;
        }

    }
}
