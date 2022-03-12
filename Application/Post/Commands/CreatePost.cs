using Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Post.Commands
{
    public class CreatePost : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImageThumb { get; set; }
        public string ImageMain { get; set; }
        public string Text { get; set; }
        public int[] Tags { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public List<Guid> PostsRelated { get; set; }

        public CreatePost(Guid id, string title, string imageThumb, string imageMain, string text, int[] tags, string createBy, DateTime createDate, DateTime updateDate, DateTime? publishDate, List<Guid> postsRelated)
        {
            Id = id;
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
