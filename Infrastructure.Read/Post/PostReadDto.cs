using Infrastructure.Core;
using System;

namespace Infrastructure.Read.Post
{
    public class PostReadDto : Dto
    {
        public string Title { get; }
        public string Text { get; }

        public PostReadDto(Guid id, string title, string text) : base(id)
        {
            Title = title;
            Text = text;
        }

    }
}
