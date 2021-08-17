using Infrastructure.Core;
using System;

namespace Infrastructure.Read.Post
{
    public class PostReadDto : Dto
    {
        public string Title { get; }
        public string Text { get; }
        public int Category { get; }
        public string ImageUrl { get; }
        public DateTime CreateDate { get; }
        public string CreateBy { get; }

        public PostReadDto(Guid id, string title, string text, int category, string imageUrl, DateTime createDate, string createBy) : base(id)
        {
            Title = title;
            Text = text;
            Category = category;
            ImageUrl = imageUrl;
            CreateDate = createDate;
            CreateBy = createBy;
        }

    }
}
