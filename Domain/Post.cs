using System;
using Domain.Core;
using Domain.Exceptions;

namespace Domain
{
    public class Post : Entity<Guid>
    {
        public string Title { get; private set; }
        public string Text { get; private set; }

        private Post(Guid id, string title, string text) : base(id)
        {
            Title = title;
            Text = text;
        }

        public static Post Create(string title, string text, Guid? postId = null)
        {
            if (postId == null)
                postId = Guid.NewGuid();

            var item = new Post(postId.Value, title, text);

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

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Title))
                throw new EmptyFieldException(nameof(Title));
        }
    }
}
