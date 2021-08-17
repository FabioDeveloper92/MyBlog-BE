using Infrastructure.Core;
using System;

namespace Infrastructure.Write.Post
{
    public class PostWriteDto : Dto
    {
        public string Title { get; }
        public string Text { get; }
        public int Category { get; }
        public string ImageUrl { get; }
        public DateTime CreateDate { get; }
        public string CreateBy { get; }

        public PostWriteDto(Guid id, string title, string text, int category, string imageUrl, DateTime createDate, string createBy) : base(id)
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
