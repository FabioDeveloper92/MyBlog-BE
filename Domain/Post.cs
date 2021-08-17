using System;
using Domain.Core;
using Domain.Exceptions;

namespace Domain
{
    public class Post : Entity<Guid>
    {
        public string Title { get; private set; }
        public string Text { get; private set; }
        public int Category { get; private set; }
        public string ImageUrl { get; private set; }
        public DateTime CreateDate { get; private set; }
        public string CreateBy { get; private set; }


        private Post(Guid id, string title, string text, int category, string imageUrl, DateTime createDate, string createBy) : base(id)
        {
            Title = title;
            Text = text;
            Category = category;
            ImageUrl = imageUrl;
            CreateDate = createDate;
            CreateBy = createBy;
        }

        public static Post Create(string title, string text, int category, string imageUrl, DateTime createDate, string createBy, Guid? postId = null)
        {
            if (postId == null)
                postId = Guid.NewGuid();

            var item = new Post(postId.Value, title, text, category, imageUrl, createDate, createBy);

            item.Validate();

            return item;
        }

        public void SetTitle(string title)
        {
            Title = title;
            Validate();
        }

        public void SetText(string text)
        {
            Text = text;
            Validate();
        }

        public void SetCategory(int category)
        {
            Category = category;
            Validate();
        }
        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            Validate();
        }
        public void SetCreateDate(DateTime createDate)
        {
            CreateDate = createDate;
            Validate();
        }
        public void SetCreateBy(string createBy)
        {
            CreateBy = createBy;
            Validate();
        }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Title))
                throw new EmptyFieldException(nameof(Title));

            if (string.IsNullOrEmpty(Text))
                throw new EmptyFieldException(nameof(Text));

            if (string.IsNullOrEmpty(ImageUrl))
                throw new EmptyFieldException(nameof(ImageUrl)); 
            
            if (string.IsNullOrEmpty(CreateBy))
                throw new EmptyFieldException(nameof(CreateBy));
        }
    }
}
