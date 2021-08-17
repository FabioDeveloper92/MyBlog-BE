using Application.Interfaces;
using System;

namespace Application.Post.Commands
{
    public class CreatePost : ICommand
    {
        public Guid PostId { get; }
        public string Title { get; }
        public string Text { get; }

        public int Category { get; }
        public string ImageUrl { get; }
        public DateTime CreateDate { get; }
        public string CreateBy { get; }

        public CreatePost(Guid postId, string title, string text, int category, string imageUrl, DateTime createDate, string createBy)
        {
            PostId = postId;
            Title = title;
            Text = text;
            Category = category;
            ImageUrl = imageUrl;
            CreateDate = createDate;
            CreateBy = createBy;
        }

    }
}
