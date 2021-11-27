using Application.Interfaces;
using System;

namespace Application.Post.Commands
{
    public class UpdatePost : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImageThumb { get; set; }
        public string ImageMain { get; set; }
        public string Text { get; set; }
        public int[] Tags { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? PublishDate { get; set; }

        public UpdatePost(Guid id, string title, string imageThumb, string imageMain, string text, int[] tags, string createBy, DateTime updateDate, DateTime? publishDate)
        {
            Id = id;
            Title = title;
            ImageThumb = imageThumb;
            ImageMain = imageMain;
            Text = text;
            Tags = tags;
            CreateBy = createBy;
            UpdateDate = updateDate;
            PublishDate = publishDate;
        }
    }
}
